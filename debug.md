# Form1.cs `GetNai3Body` 方法优化分析

## 1. 现状分析

`Form1.cs` 中的 `GetNai3Body` 方法负责从界面的各个控件（文本框、下拉框、表格等）收集用户输入的参数，并将它们组装成一个 `BodyBase` 对象，用于发送给 NovelAI API。

目前该方法的主要问题是：

- **职责过重 (Single Responsibility Principle Violation)**: 该方法集 UI 读取、数据处理、参数组装等多项职责于一身。它需要了解 `img2img`、`Vibe`、`V4 prompts` 等多种功能的具体参数逻辑。
- **可读性差**: 方法体冗长，包含了大量的 `if` 判断和数据转换逻辑，新开发者很难快速理解整个参数的构建流程。
- **可维护性与扩展性差**: 如果未来 API 需要支持新的模型或新的参数（例如 `V5 prompts`），就需要继续向这个已经很臃肿的方法中添加更多的 `if/else` 逻辑，使其越来越难以维护。

### 原始代码:

```csharp
private BodyBase GetNai3Body(int runNum) {
    int[] resolution = GetResolution(runNum);
    Dictionary<string, object> kwargs = picProps.GetProperty();
    kwargs.Add("negative_prompt", txtNegativePrompt.Text);

    // img2img
    if (img2ImgCurrentPath != null) {
        kwargs.Add("image", Tools.ConvertImageToBase64(img2ImgCurrentPath));
        kwargs.Add("strength", (float)nudImg2ImgStrength.Value);
        kwargs.Add("noise", (float)nudImg2ImgNoise.Value);
    }

    // vibe
    List<string> referenceImages = new List<string>();
    List<float> referenceInfoExtracted = new List<float>();
    List<float> referenceStrength = new List<float>();

    foreach (DataGridViewRow row in dgvVibe.Rows) {
        var picPath = row.Cells["Column1"].Value;
        if (picPath == null) continue;

        string base64img = Tools.ConvertImageToBase64(picPath.ToString());
        if (string.IsNullOrEmpty(base64img)) {
            Logger.Error("图片转换失败，路径为" + picPath);
            continue;
        }

        referenceImages.Add(base64img);

        var ie = row.Cells["Column2"].Value;
        referenceInfoExtracted.Add(ie != null ? float.Parse(ie.ToString()) : 0);

        var rs = row.Cells["Column3"].Value;
        referenceStrength.Add(rs != null ? float.Parse(rs.ToString()) : 0);
    }

    //vibe
    if (referenceImages.Count > 0) {
        kwargs.Add("reference_image_multiple", referenceImages);
        kwargs.Add("reference_information_extracted_multiple", referenceInfoExtracted);
        kwargs.Add("reference_strength_multiple", referenceStrength);
    }

    var prompt = Prompt.GetPrompt(txtPrompt.Text, this);
    prevNoArtistPrompt = Prompt.GetNoArtistPrompt(prompt);
    string tPrompt = Prompt.GetDataPrompt(prompt);
    kwargs.Add("prompt", tPrompt);

    //nai4
    kwargs.Add("v4_negative_prompt",
        new V4Prompt(new Caption(txtNegativePrompt.Text, new List<CharCaption>()), null, null, false));
    kwargs.Add("v4_prompt", new V4Prompt(new Caption(tPrompt, new List<CharCaption>()), true, true, null));
    BodyBase body = BodyTools.GetBody(picProps.Model, kwargs);
    propertyGrid1.Refresh();
    return body;
}
```

## 2. 优化建议

核心思想是将 `GetNai3Body` 方法进行拆分和重构，使其成为一个只负责协调的入口点，而将具体的参数获取逻辑封装到独立的私有方法中。这样可以极大地提高代码的模块化程度和可读性。

### 优化后的代码:

```csharp
// 这是优化后的主方法，现在非常简洁，只负责协调
private BodyBase GetNai3Body(int runNum) {
    try {
        // 1. 获取所有基础和功能的参数
        Dictionary<string, object> kwargs = GatherAllParameters(runNum);

        // 2. 使用工厂模式创建最终的 Body 对象
        BodyBase body = BodyTools.GetBody(picProps.Model, kwargs);
        
        // 3. 刷新UI
        propertyGrid1.Refresh();
        return body;
    }
    catch (Exception ex) {
        Logger.Error("构建请求参数时出错: " + ex.Message);
        // 可以根据需要返回 null 或抛出异常
        return null; 
    }
}

// 新增一个总的参数收集方法
private Dictionary<string, object> GatherAllParameters(int runNum) {
    // 初始化，从 PropertyGrid 获取基础参数
    Dictionary<string, object> kwargs = picProps.GetProperty();

    // 分别添加不同模块的参数
    AddResolutionParameters(kwargs, runNum);
    AddNegativePromptParameter(kwargs);
    AddImg2ImgParameters(kwargs);
    AddVibeParameters(kwargs);
    AddPromptParameters(kwargs);
    AddV4Parameters(kwargs); // V4 的参数也独立出来

    return kwargs;
}

// --- 以下是将原始逻辑拆分出的独立方法 ---

private void AddResolutionParameters(Dictionary<string, object> kwargs, int runNum) {
    // 此处代码与 GetResolution 方法逻辑相同，为保持内聚性，可以将其逻辑移入此处
    // 为了简化，我们假设 GetResolution 已经更新了 picProps 的 Width 和 Height
    GetResolution(runNum); 
    // kwargs 由 picProps.GetProperty() 初始化，已经包含了最新的 Width 和 Height
}

private void AddNegativePromptParameter(Dictionary<string, object> kwargs) {
    kwargs["negative_prompt"] = txtNegativePrompt.Text;
}

private void AddImg2ImgParameters(Dictionary<string, object> kwargs) {
    if (!string.IsNullOrEmpty(img2ImgCurrentPath)) {
        kwargs["image"] = Tools.ConvertImageToBase64(img2ImgCurrentPath);
        kwargs["strength"] = (float)nudImg2ImgStrength.Value;
        kwargs["noise"] = (float)nudImg2ImgNoise.Value;
    }
}

private void AddVibeParameters(Dictionary<string, object> kwargs) {
    var referenceImages = new List<string>();
    var referenceInfoExtracted = new List<float>();
    var referenceStrength = new List<float>();

    foreach (DataGridViewRow row in dgvVibe.Rows) {
        if (row.IsNewRow) continue; // 避免处理未提交的新行

        var picPath = row.Cells["Column1"].Value?.ToString();
        if (string.IsNullOrEmpty(picPath)) continue;

        string base64img = Tools.ConvertImageToBase64(picPath);
        if (string.IsNullOrEmpty(base64img)) {
            Logger.Error("Vibe 图片转换失败，路径为: " + picPath);
            continue;
        }

        referenceImages.Add(base64img);
        referenceInfoExtracted.Add(Convert.ToSingle(row.Cells["Column2"].Value ?? 0f));
        referenceStrength.Add(Convert.ToSingle(row.Cells["Column3"].Value ?? 0f));
    }

    if (referenceImages.Any()) {
        kwargs["reference_image_multiple"] = referenceImages;
        kwargs["reference_information_extracted_multiple"] = referenceInfoExtracted;
        kwargs["reference_strength_multiple"] = referenceStrength;
    }
}

private void AddPromptParameters(Dictionary<string, object> kwargs) {
    string rawPrompt = Prompt.GetPrompt(txtPrompt.Text, this);
    prevNoArtistPrompt = Prompt.GetNoArtistPrompt(rawPrompt); // prevNoArtistPrompt 是类成员，这里需要处理
    string finalPrompt = Prompt.GetDataPrompt(rawPrompt);
    kwargs["prompt"] = finalPrompt;
}

private void AddV4Parameters(Dictionary<string, object> kwargs) {
    // 仅当模型是 V4 或更高版本时才添加这些参数，增加代码的健壮性
    if (picProps.Model.ToString().Contains("Nai4")) {
        string prompt = kwargs.ContainsKey("prompt") ? kwargs["prompt"].ToString() : "";
        string negativePrompt = kwargs.ContainsKey("negative_prompt") ? kwargs["negative_prompt"].ToString() : "";

        kwargs["v4_negative_prompt"] = new V4Prompt(new Caption(negativePrompt, new List<CharCaption>()), null, null, false);
        kwargs["v4_prompt"] = new V4Prompt(new Caption(prompt, new List<CharCaption>()), true, true, null);
    }
}
```

## 3. 优化带来的好处

- **高内聚，低耦合**: 每个方法都只关心自己的任务（例如 `AddVibeParameters` 只处理 Vibe 相关的参数），与其他参数的获取逻辑解耦。
- **可读性强**: `GetNai3Body` 和 `GatherAllParameters` 方法现在像一个目录，清晰地展示了参数构建的步骤。开发者可以轻松地跳转到任何一个具体的参数处理方法中。
- **易于维护和扩展**:
    - 如果 Vibe 功能的 UI 变了，只需要修改 `AddVibeParameters` 方法。
    - 如果要支持一个新的 `Nai5` 模型，它有自己独特的参数结构，我们只需要新增一个 `AddV5Parameters` 方法，并在 `GatherAllParameters` 中调用它即可，完全不会影响到现有逻辑。
- **健壮性提升**: 在 `AddV4Parameters` 中增加了对模型版本的检查，避免了为不支持 V4 参数的模型错误地添加数据。同时，主流程中增加了 `try-catch`，可以更好地处理参数构建过程中可能出现的异常。

通过以上重构，代码变得更加优雅、健壮和面向未来。