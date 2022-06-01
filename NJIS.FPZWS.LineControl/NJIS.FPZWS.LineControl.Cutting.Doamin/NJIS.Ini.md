### 使用方法

~~~ C#
[IniFile("TestSetting")]
 public class TestSetting : SettingBase<TestSetting>
{
    [Property("db")]
    public string Name { get; set; } = "ttt";

    public string Sex { get; set; }
}
~~~
> 1. 配置类必须继承SettingBase< T >，T为类本身
> 2. 属性访问级别必须为public，通过特性Property指定ini文件section、key节点，其中section如果不指定默认为general，Key 默认为属性米名称
> 3. IniFile 用于配置映射的ini文件名称，默认为类名