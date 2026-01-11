using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoNai3Tools.utils {
    public class AutoCompleteHelper : IDisposable {
        private readonly TextBox _textBox;
        private readonly ListBox _listBox;
        private readonly TagDatabase _tagDatabase;
        private readonly Func<IEnumerable<string>> _wildcardProvider;
        private readonly string[] _staticTags = { "<固定画师>", "<随机画师>", "<随机提示词>" };
        
        private readonly System.Windows.Forms.Timer _debounceTimer;
        private CancellationTokenSource _searchCts;
        private bool _disposed;

        public AutoCompleteHelper(TextBox textBox, TagDatabase tagDatabase, Func<IEnumerable<string>> wildcardProvider) {
            _textBox = textBox;
            _tagDatabase = tagDatabase;
            _wildcardProvider = wildcardProvider;

            _listBox = new ListBox {
                Visible = false,
                Height = 200,
                Width = 300,
                ScrollAlwaysVisible = true,
                Font = new Font("Consolas", 10),
                BorderStyle = BorderStyle.FixedSingle,
                IntegralHeight = false
            };

            if (_textBox.Parent != null) {
                _textBox.Parent.Controls.Add(_listBox);
                _listBox.BringToFront();
            }

            _debounceTimer = new System.Windows.Forms.Timer { Interval = 100 }; // 100ms debounce
            _debounceTimer.Tick += DebounceTimer_Tick;

            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.KeyUp += TextBox_KeyUp;
            
            _textBox.Leave += TextBox_Leave;

            _listBox.KeyDown += ListBox_KeyDown;
            _listBox.DoubleClick += (s, e) => ConfirmSelection();
            _listBox.LostFocus += ListBox_LostFocus;
        }

        private void TextBox_Leave(object sender, EventArgs e) {
             if (!_listBox.Focused) _listBox.Visible = false;
        }

        private void ListBox_LostFocus(object sender, EventArgs e) {
             if (!_textBox.Focused) _listBox.Visible = false;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || 
                e.KeyCode == Keys.Up || e.KeyCode == Keys.Tab || 
                e.KeyCode == Keys.Left || e.KeyCode == Keys.Right) return;
            
            // Restart timer
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void DebounceTimer_Tick(object sender, EventArgs e) {
            _debounceTimer.Stop();
            TriggerSearch();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if (_listBox.Visible && _listBox.Items.Count > 0) {
                if (e.KeyCode == Keys.Down) {
                    int newIndex = _listBox.SelectedIndex + 1;
                    if (newIndex >= _listBox.Items.Count) newIndex = 0;
                    _listBox.SelectedIndex = newIndex;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                } else if (e.KeyCode == Keys.Up) {
                    int newIndex = _listBox.SelectedIndex - 1;
                    if (newIndex < 0) newIndex = _listBox.Items.Count - 1;
                    _listBox.SelectedIndex = newIndex;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                } else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab) {
                    ConfirmSelection();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                } else if (e.KeyCode == Keys.Escape) {
                    _listBox.Visible = false;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e) {
             if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab) {
                ConfirmSelection();
                e.Handled = true;
                e.SuppressKeyPress = true;
                _textBox.Focus();
            } else if (e.KeyCode == Keys.Escape) {
                _listBox.Visible = false;
                _textBox.Focus();
            }
        }

        private async void TriggerSearch() {
            if (_disposed) return;

            // Cancel previous running search
            if (_searchCts != null) {
                _searchCts.Cancel();
                _searchCts.Dispose();
                _searchCts = null;
            }
            
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;

            try {
                string currentWord = GetCurrentWord(out int startIndex);
                if (string.IsNullOrWhiteSpace(currentWord) || currentWord.Length < 1) {
                    if (!token.IsCancellationRequested) _listBox.Visible = false;
                    return;
                }

                var suggestions = await Task.Run(() => {
                    var results = new HashSet<string>();

                    foreach (var t in _staticTags) {
                        if (token.IsCancellationRequested) return null;
                        if (t.IndexOf(currentWord, StringComparison.OrdinalIgnoreCase) >= 0) results.Add(t);
                    }
                    
                    var wildcards = _wildcardProvider();
                    if (wildcards != null) {
                        foreach (var t in wildcards) {
                            if (token.IsCancellationRequested) return null;
                            if (t.IndexOf(currentWord, StringComparison.OrdinalIgnoreCase) >= 0) results.Add(t);
                        }
                    }

                    if (!currentWord.StartsWith("<")) {
                        var dbTags = _tagDatabase.SearchTags(currentWord);
                        foreach (var t in dbTags) {
                            if (token.IsCancellationRequested) return null;
                            results.Add(t);
                        }
                    }
                    
                    return results.OrderBy(s => s).ToList();
                }, token);

                if (token.IsCancellationRequested || suggestions == null || _disposed) return;

                if (suggestions.Count == 0) {
                    _listBox.Visible = false;
                    return;
                }

                _listBox.BeginUpdate();
                _listBox.DataSource = suggestions;
                _listBox.EndUpdate();

                // Positioning
                Point pt = _textBox.GetPositionFromCharIndex(startIndex);
                pt.Y += (int)_textBox.Font.GetHeight() + 2;
                Point loc = _textBox.Location;
                loc.Offset(pt);
                
                if (loc.X + _listBox.Width > _textBox.Parent.Width) {
                    loc.X = _textBox.Parent.Width - _listBox.Width;
                }
                
                _listBox.Location = loc;
                if (!_listBox.Visible) {
                    _listBox.Visible = true;
                    _listBox.BringToFront();
                }
                
            } catch (OperationCanceledException) {
                // Expected
            } catch (Exception ex) {
                Logger.Warn($"AutoComplete Error: {ex.Message}");
            }
        }

        private string GetCurrentWord(out int startIndex) {
            int pos = _textBox.SelectionStart;
            string text = _textBox.Text;
            
            int start = pos - 1;
            while (start >= 0) {
                char c = text[start];
                if (c == ',' || c == '{' || c == '}' || c == '(' || c == ')') break;
                start--;
            }
            start++;
            
            while (start < pos && char.IsWhiteSpace(text[start])) {
                start++;
            }

            startIndex = start;
            if (startIndex >= pos) return "";
            return text.Substring(startIndex, pos - startIndex);
        }

        private void ConfirmSelection() {
            if (_listBox.SelectedItem == null) return;
            string textToInsert = _listBox.SelectedItem.ToString();
            
            string currentWord = GetCurrentWord(out int startIndex);
            
            _textBox.Select(startIndex, currentWord.Length);
            _textBox.SelectedText = textToInsert;
            _listBox.Visible = false;
            _textBox.Focus();
            _textBox.SelectionStart = startIndex + textToInsert.Length;
            _textBox.SelectionLength = 0;
        }

        public void Dispose() {
            if (_disposed) return;
            _disposed = true;
            
            _debounceTimer?.Stop();
            _debounceTimer?.Dispose();
            
            if (_searchCts != null) {
                _searchCts.Cancel();
                _searchCts.Dispose();
            }
            
            if (_listBox != null && !_listBox.IsDisposed) {
                _listBox.Dispose();
            }
            
            // Unsubscribe events to prevent memory leaks if AutoCompleteHelper is recreated but controls live on
            _textBox.KeyDown -= TextBox_KeyDown;
            _textBox.KeyUp -= TextBox_KeyUp;
            _textBox.Leave -= TextBox_Leave;
        }
    }
}