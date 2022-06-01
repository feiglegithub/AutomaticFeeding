using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NJIS.FPZWS.Authority.Contract;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.Design;
using Telerik.WinControls.Layouts;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace NJIS.Windows.TemplateBase.AuthorityControl
{
    [ToolboxItem(false), ComVisible(false)]
    public class AuthorityRibbonBarButton : RadButtonItem, IAuthorityControl
    {
        public static RadProperty LargeImageProperty = RadProperty.Register("LargeImage", typeof(Image), typeof(RadButtonElement), new RadElementPropertyMetadata(null, ElementPropertyOptions.AffectsLayout | ElementPropertyOptions.InvalidatesLayout));
        public static RadProperty LargeImageIndexProperty = RadProperty.Register("LargeImageIndex", typeof(int), typeof(RadButtonElement), new RadElementPropertyMetadata(-1, ElementPropertyOptions.AffectsLayout | ElementPropertyOptions.InvalidatesLayout));
        public static RadProperty LargeImageKeyProperty = RadProperty.Register("LargeImageKey", typeof(string), typeof(RadButtonElement), new RadElementPropertyMetadata(string.Empty, ElementPropertyOptions.None));
        public static RadProperty SmallImageProperty = RadProperty.Register("SmallImage", typeof(Image), typeof(RadButtonElement), new RadElementPropertyMetadata(null, ElementPropertyOptions.AffectsLayout | ElementPropertyOptions.InvalidatesLayout));
        public static RadProperty SmallImageIndexProperty = RadProperty.Register("SmallImageIndex", typeof(int), typeof(RadButtonElement), new RadElementPropertyMetadata(-1, ElementPropertyOptions.AffectsLayout | ElementPropertyOptions.InvalidatesLayout));
        public static RadProperty SmallImageKeyProperty = RadProperty.Register("SmallImageKey", typeof(string), typeof(RadButtonElement), new RadElementPropertyMetadata(string.Empty, ElementPropertyOptions.None));
        public static RadProperty UseSmallImageListProperty = RadProperty.Register("UseSmallImageList", typeof(bool), typeof(RadButtonElement), new RadElementPropertyMetadata(false, ElementPropertyOptions.None));

        private TextPrimitive textPrimitive;
        private FillPrimitive fillPrimitive;
        private BorderPrimitive borderPrimitive;
        private Telerik.WinControls.Primitives.FocusPrimitive focusPrimitive;
        private ImageAndTextLayoutPanel layoutPanel;
        private Telerik.WinControls.Primitives.ImagePrimitive imagePrimitive;

        static AuthorityRibbonBarButton()
        {
            RadElement.CanFocusProperty.OverrideMetadata(typeof(RadButtonElement), new RadElementPropertyMetadata(true, ElementPropertyOptions.AffectsDisplay));
        }

        public AuthorityRibbonBarButton()
        {
        }

        public AuthorityRibbonBarButton(string text) : base(text)
        {
        }

        public AuthorityRibbonBarButton(string text, Image image) : base(text, image)
        {
        }

        protected override void CreateChildElements()
        {
            this.fillPrimitive = new FillPrimitive();
            this.fillPrimitive.Class = "ButtonFill";
            this.borderPrimitive = new BorderPrimitive();
            this.borderPrimitive.Class = "ButtonBorder";
            this.borderPrimitive.ZIndex = 2;
            this.borderPrimitive.BindProperty(RadElement.IsItemFocusedProperty, this, RadElement.IsFocusedProperty, PropertyBindingOptions.OneWay);
            this.textPrimitive = new TextPrimitive();
            this.textPrimitive.SetValue(ImageAndTextLayoutPanel.IsTextPrimitiveProperty, true);
            this.textPrimitive.BindProperty(TextPrimitive.TextProperty, this, RadItem.TextProperty, PropertyBindingOptions.OneWay);
            this.textPrimitive.BindProperty(TextPrimitive.TextAlignmentProperty, this, RadButtonItem.TextAlignmentProperty, PropertyBindingOptions.OneWay);
            this.imagePrimitive = new Telerik.WinControls.Primitives.ImagePrimitive();
            this.imagePrimitive.SetValue(ImageAndTextLayoutPanel.IsImagePrimitiveProperty, true);
            this.imagePrimitive.BindProperty(Telerik.WinControls.Primitives.ImagePrimitive.ImageIndexProperty, this, RadButtonItem.ImageIndexProperty, PropertyBindingOptions.TwoWay);
            this.imagePrimitive.BindProperty(Telerik.WinControls.Primitives.ImagePrimitive.ImageProperty, this, RadButtonItem.ImageProperty, PropertyBindingOptions.TwoWay);
            this.imagePrimitive.BindProperty(Telerik.WinControls.Primitives.ImagePrimitive.ImageKeyProperty, this, RadButtonItem.ImageKeyProperty, PropertyBindingOptions.TwoWay);
            this.imagePrimitive.BindProperty(Telerik.WinControls.Primitives.ImagePrimitive.UseSmallImageListProperty, this, UseSmallImageListProperty, PropertyBindingOptions.TwoWay);
            this.imagePrimitive.BindProperty(RadElement.AlignmentProperty, this, RadButtonItem.ImageAlignmentProperty, PropertyBindingOptions.OneWay);
            this.layoutPanel = new ImageAndTextLayoutPanel();
            this.layoutPanel.ZIndex = 3;
            this.layoutPanel.BindProperty(RadElement.StretchHorizontallyProperty, this, RadElement.StretchHorizontallyProperty, PropertyBindingOptions.OneWay);
            this.layoutPanel.BindProperty(RadElement.StretchVerticallyProperty, this, RadElement.StretchVerticallyProperty, PropertyBindingOptions.OneWay);
            this.layoutPanel.BindProperty(ImageAndTextLayoutPanel.DisplayStyleProperty, this, RadButtonItem.DisplayStyleProperty, PropertyBindingOptions.OneWay);
            this.layoutPanel.BindProperty(ImageAndTextLayoutPanel.ImageAlignmentProperty, this, RadButtonItem.ImageAlignmentProperty, PropertyBindingOptions.OneWay);
            this.layoutPanel.BindProperty(ImageAndTextLayoutPanel.TextAlignmentProperty, this, RadButtonItem.TextAlignmentProperty, PropertyBindingOptions.OneWay);
            this.layoutPanel.BindProperty(ImageAndTextLayoutPanel.TextImageRelationProperty, this, RadButtonItem.TextImageRelationProperty, PropertyBindingOptions.OneWay);
            this.layoutPanel.Children.Add(this.imagePrimitive);
            this.layoutPanel.Children.Add(this.textPrimitive);
            this.focusPrimitive = new Telerik.WinControls.Primitives.FocusPrimitive(this.borderPrimitive);
            this.focusPrimitive.Class = "ButtonFocus";
            this.focusPrimitive.ZIndex = 4;
            this.focusPrimitive.Visibility = ElementVisibility.Hidden;
            this.focusPrimitive.BindProperty(RadElement.IsItemFocusedProperty, this, RadElement.IsFocusedProperty, PropertyBindingOptions.OneWay);
            this.Children.Add(this.fillPrimitive);
            this.Children.Add(this.layoutPanel);
            this.Children.Add(this.borderPrimitive);
            this.Children.Add(this.focusPrimitive);
        }

        public override void DpiScaleChanged(SizeF scaleFactor)
        {
            base.DpiScaleChanged(scaleFactor);
        }

        public override VisualStyleElement GetVistaVisualStyle() =>
            this.GetXPVisualStyle();

        public override VisualStyleElement GetXPVisualStyle()
        {
            if (!this.Enabled)
            {
                return VisualStyleElement.Button.PushButton.Disabled;
            }
            if (base.IsMouseDown)
            {
                if (!base.IsMouseOver)
                {
                    return VisualStyleElement.Button.PushButton.Hot;
                }
                return VisualStyleElement.Button.PushButton.Pressed;
            }
            if (base.IsMouseOver)
            {
                return VisualStyleElement.Button.PushButton.Hot;
            }
            if (!this.IsDefault)
            {
                return VisualStyleElement.Button.PushButton.Normal;
            }
            return VisualStyleElement.Button.PushButton.Default;
        }

        protected override void InitializeFields()
        {
            base.InitializeFields();
            this.CanFocus = true;
        }

        protected override void InitializeSystemSkinPaint()
        {
            base.InitializeSystemSkinPaint();
            this.textPrimitive.ForeColor = SystemSkinManager.Instance.Renderer.GetColor(ColorProperty.TextColor);
        }

        protected SizeF MeasureButtonChildren(SizeF availableSize)
        {
            SizeF empty = SizeF.Empty;
            if (this.AutoSize)
            {
                for (int j = 0; j < this.Children.Count; j++)
                {
                    RadElement element = this.Children[j];
                    if ((element.FitToSizeMode == RadFitToSizeMode.FitToParentContent) || (element.FitToSizeMode == RadFitToSizeMode.FitToParentPadding))
                    {
                        element.Measure(SizeF.Subtract(SizeF.Subtract(availableSize, (SizeF)this.Padding.Size), (SizeF)this.BorderThickness.Size));
                    }
                    else
                    {
                        element.Measure(availableSize);
                    }
                    SizeF desiredSize = element.DesiredSize;
                    if (element.FitToSizeMode == RadFitToSizeMode.FitToParentContent)
                    {
                        desiredSize = SizeF.Add(desiredSize, (SizeF)this.Padding.Size);
                    }
                    if ((element.FitToSizeMode == RadFitToSizeMode.FitToParentContent) || (element.FitToSizeMode == RadFitToSizeMode.FitToParentPadding))
                    {
                        SizeF ef3 = SizeF.Add(desiredSize, (SizeF)this.BorderThickness.Size);
                        float width = ef3.Width;
                        float height = ef3.Height;
                        if ((this.ElementTree.RootElement.MaxSize.Width > 0) && (ef3.Width > this.ElementTree.Control.MaximumSize.Width))
                        {
                            width = this.ElementTree.Control.MaximumSize.Width;
                        }
                        if ((this.MaxSize.Width > 0) && (ef3.Width > this.MaxSize.Width))
                        {
                            width = this.MaxSize.Width;
                        }
                        if ((this.ElementTree.RootElement.MaxSize.Height > 0) && (ef3.Height > this.ElementTree.Control.MaximumSize.Height))
                        {
                            height = this.ElementTree.Control.MaximumSize.Height;
                        }
                        if ((this.MaxSize.Height > 0) && (ef3.Height > this.MaxSize.Height))
                        {
                            height = this.MaxSize.Height;
                        }
                        desiredSize = new SizeF(width, height);
                    }
                    empty.Width = Math.Min(availableSize.Width, Math.Max(empty.Width, desiredSize.Width));
                    empty.Height = Math.Min(availableSize.Height, Math.Max(empty.Height, desiredSize.Height));
                }
                return empty;
            }
            for (int i = 0; i < this.Children.Count; i++)
            {
                this.Children[i].Measure(availableSize);
            }
            return (SizeF)this.Size;
        }

        protected override SizeF MeasureOverride(SizeF availableSize) =>
            this.MeasureButtonChildren(availableSize);

        protected override void OnClick(EventArgs e)
        {
            MouseEventArgs args = e as MouseEventArgs;
            if ((args == null) || (args.Button == MouseButtons.Left))
            {
                base.OnClick(e);
                if (((this.ElementTree != null) && (this.ElementTree.Control != null)) && (this.textPrimitive != null))
                {
                    RadControl control = this.ElementTree.Control as RadControl;
                    ControlTraceMonitor.TrackAtomicFeature(control, "Click", this.textPrimitive.Text);
                }
            }
        }


        protected override bool ShouldPaintChild(RadElement element) =>
            (((base.paintSystemSkin != true) || ((element != this.fillPrimitive) && (element != this.borderPrimitive))) && base.ShouldPaintChild(element));

        public void SetValue(bool holdAuthority)
        {
            throw new NotImplementedException();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(true)]
        public FillPrimitive ButtonFillElement
        {
            get
            {
                return this.fillPrimitive;
            }
            protected set
            {
                this.fillPrimitive = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(true)]
        public BorderPrimitive BorderElement
        {
            get { return this.borderPrimitive; }
            protected set
            {
                this.borderPrimitive = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(true)]
        public TextPrimitive TextElement
        {
            get
            {
                return this.textPrimitive;
            }
            protected set
            {
                this.textPrimitive = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Telerik.WinControls.Primitives.ImagePrimitive ImagePrimitive
        {
            get
            {
                return
             this.imagePrimitive;
            }
            protected set
            {
                this.imagePrimitive = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Telerik.WinControls.Primitives.FocusPrimitive FocusPrimitive =>
            this.focusPrimitive;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ImageAndTextLayoutPanel LayoutPanel
        {
            get
            {
                return
             this.layoutPanel;
            }
            protected set
            {
                this.layoutPanel = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance"), RadPropertyDefaultValue("LargeImage", typeof(RadButtonElement)), Description("Gets the large image that is displayed on a button element."), TypeConverter(typeof(ImageTypeConverter))]
        public virtual Image LargeImage =>
            ((Image)this.GetValue(LargeImageProperty));

        [TypeConverter("Telerik.WinControls.UI.Design.NoneExcludedImageIndexConverter, Telerik.WinControls.UI.Design, Version=2017.2.502.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance"), Description("Gets the large image list index value of the image displayed on the button control."), RadPropertyDefaultValue("LargeImageIndex", typeof(RadButtonElement)), RelatedImageList("ElementTree.Control.ImageList"), Editor("Telerik.WinControls.UI.Design.ImageIndexEditor, Telerik.WinControls.UI.Design, Version=2017.2.502.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e", typeof(UITypeEditor))]
        public virtual int LargeImageIndex =>
            ((int)this.GetValue(LargeImageIndexProperty));

        [Description("Gets the large key accessor for the image in the ImageList."), RadPropertyDefaultValue("LargeImageKey", typeof(RadButtonElement)), Category("Appearance"), Editor("Telerik.WinControls.UI.Design.ImageKeyEditor, Telerik.WinControls.UI.Design, Version=2017.2.502.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e", typeof(UITypeEditor)), Browsable(false), TypeConverter("Telerik.WinControls.UI.Design.RadImageKeyConverter, Telerik.WinControls.UI.Design, Version=2017.2.502.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string LargeImageKey =>
            ((string)this.GetValue(LargeImageKeyProperty));

        [Category("Appearance"), Description("Gets or sets the large image that is displayed on a button element."), TypeConverter(typeof(ImageTypeConverter)), RefreshProperties(RefreshProperties.All), RadPropertyDefaultValue("SmallImage", typeof(RadButtonElement))]
        public virtual Image SmallImage
        {
            get
            {
                return
             ((Image)this.GetValue(SmallImageProperty));
            }
            set
            {
                base.SetValue(SmallImageProperty, value);
            }
        }

        [TypeConverter("Telerik.WinControls.UI.Design.NoneExcludedImageIndexConverter, Telerik.WinControls.UI.Design, Version=2017.2.502.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e"), RadPropertyDefaultValue("SmallImageIndex", typeof(RadButtonElement)), RelatedImageList("ElementTree.Control.SmallImageList"), Editor("Telerik.WinControls.UI.Design.ImageIndexEditor, Telerik.WinControls.UI.Design, Version=2017.2.502.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e", typeof(UITypeEditor)), Description("Gets or sets the small image list index value of the image displayed on the button control."), RefreshProperties(RefreshProperties.All), Category("Appearance")]
        public virtual int SmallImageIndex
        {
            get
            {
                return
             ((int)this.GetValue(SmallImageIndexProperty));
            }
            set
            {
                base.SetValue(SmallImageIndexProperty, value);
            }
        }

        [TypeConverter("Telerik.WinControls.UI.Design.RadImageKeyConverter, Telerik.WinControls.UI.Design, Version=2017.2.502.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e"), Category("Appearance"), Editor("Telerik.WinControls.UI.Design.ImageKeyEditor, Telerik.WinControls.UI.Design, Version=2017.2.502.40, Culture=neutral, PublicKeyToken=5bb2a467cbec794e", typeof(UITypeEditor)), RadPropertyDefaultValue("SmallImageKey", typeof(RadButtonElement)), RelatedImageList("ElementTree.Control.SmallImageList"), RefreshProperties(RefreshProperties.All), Description("Gets or sets the small key accessor for the image in the ImageList.")]
        public virtual string SmallImageKey
        {
            get
            {
                return
             ((string)this.GetValue(SmallImageKeyProperty));
            }
            set
            {
                base.SetValue(SmallImageKeyProperty, value);
            }
        }

        [Browsable(false)]
        public virtual bool UseSmallImageList
        {
            get
            {
                return
             ((bool)this.GetValue(UseSmallImageListProperty));
            }
            set
            {
                base.SetValue(UseSmallImageListProperty, value);
            }
        }
        [RadDefaultValue("AngleTransform", typeof(Telerik.WinControls.Primitives.ImagePrimitive)), Description("Angle of rotation for the button image"), Category("Behavior")]
        public float ImagePrimitiveAngleTransform
        {
            get
            {
                if (this.imagePrimitive != null)
                {
                    return this.imagePrimitive.AngleTransform;
                }
                return 0f;
            }
            set
            {
                if (this.imagePrimitive != null)
                {
                    this.imagePrimitive.AngleTransform = value;
                }
            }
        }

        [Category("Appearance"), Description("True if the text should wrap to the available layout rectangle otherwise, false."), RadPropertyDefaultValue("TextWrap", typeof(TextPrimitive))]
        public bool TextWrap
        {
            get
            {
                return
             this.textPrimitive.TextWrap;
            }
            set
            {
                this.textPrimitive.TextWrap = value;
            }
        }
        [DefaultValue(true), Localizable(true), Description("Includes the trailing space at the end of each line. By default the boundary rectangle returned by the Overload:System.Drawing.Graphics.MeasureString method excludes the space at the end of each line. Set this flag to include that space in measurement."), Category("Appearance")]
        public bool MeasureTrailingSpaces
        {
            get
            {
                return
             this.textPrimitive.MeasureTrailingSpaces;
            }
            set
            {
                this.textPrimitive.MeasureTrailingSpaces = value;
            }
        }
        [DefaultValue(true), Browsable(true), Category("Behavior"), Description("Gets or sets a value indicating whether the border is shown.")]
        public bool ShowBorder
        {
            get
            {
                return
                    (this.borderPrimitive.Visibility == ElementVisibility.Visible);
            }

            set
            {
                this.borderPrimitive.Visibility = value ? ElementVisibility.Visible : ElementVisibility.Collapsed;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsCancelClicked
        {
            get
            {
                return
                    base.BitState[0x400000000000L];
            }
            set
            {
                this.SetBitState(0x400000000000L, value);
            }
        }

        [Browsable(false)]
        public string AuthorityPath => this.GetType().ReflectedType.FullName;

        [Browsable(false)]
        public string InstanceName => this.GetType().FullName;

        public bool IsAuthorityControl { get; set; }
    }
}
