namespace Saturn.Views.TemplateSelectors;

public class ProductUISelector : DataTemplateSelector
{
    public DataTemplate DefaultTemplate { get; set; }
    public DataTemplate SecondTemplate { get; set; }
    public DataTemplate ThirdTemplate { get; set; }
    public DataTemplate FourthTemplate { get; set; }
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var obj = (Product)item;
        if (obj.ProductId % 2 == 0) return SecondTemplate;
        else if (obj.ProductId % 3 == 0) return ThirdTemplate;
        else if (obj.ProductId % 5 == 0) return FourthTemplate;
        else return DefaultTemplate;
    }
}
