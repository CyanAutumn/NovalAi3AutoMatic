using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace AutoNai3Tools.utils {
    public class TagDatabase : IDisposable {
        private readonly string _dbPath;
        private SQLiteConnection _connection;
        private readonly object _lock = new object();

        public TagDatabase(string dbPath) {
            _dbPath = dbPath;
            InitializeConnection();
        }

        private void InitializeConnection() {
            if (!File.Exists(_dbPath)) return;

            try {
                var builder = new SQLiteConnectionStringBuilder {
                    DataSource = _dbPath,
                    Version = 3,
                    ReadOnly = true,
                    Pooling = true
                };
                
                _connection = new SQLiteConnection(builder.ToString());
                _connection.Open();
            } catch (Exception ex) {
                Logger.Warn($"TagDB Init Error: {ex.Message}");
            }
        }

        public List<string> SearchTags(string keyword, int limit = 20) {
            var results = new List<string>();
            if (string.IsNullOrWhiteSpace(keyword)) return results;
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open) {
                // Try to reconnect if connection is lost
                 lock (_lock) {
                    if (_connection == null || _connection.State != System.Data.ConnectionState.Open) {
                         InitializeConnection();
                    }
                 }
                 if (_connection == null || _connection.State != System.Data.ConnectionState.Open) return results;
            }

            try {
                lock (_lock) { // SQLite connection is not thread-safe by default unless configured
                    string query = "SELECT tag FROM tag_catalog WHERE tag LIKE @keyword OR REPLACE(tag, ' ', '') LIKE @keyword_nospace ORDER BY count DESC LIMIT @limit";
                    using (var cmd = new SQLiteCommand(query, _connection)) {
                        cmd.Parameters.AddWithValue("@keyword", keyword + "%");
                        cmd.Parameters.AddWithValue("@keyword_nospace", keyword.Replace(" ", "") + "%");
                        cmd.Parameters.AddWithValue("@limit", limit);
                        using (var reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                if (!reader.IsDBNull(0)) {
                                    results.Add(reader.GetString(0));
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Logger.Warn($"TagDB Search Error: {ex.Message}", context: Logger.Context(("keyword", keyword)));
            }
            return results;
        }

        public void Dispose() {
            lock (_lock) {
                if (_connection != null) {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
    }
}
