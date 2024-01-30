namespace Saturn.Views.TemplateSelectors;

public class ChatUISelector : DataTemplateSelector
{
    public DataTemplate SenderMessageTemplate {get; set;}
    public DataTemplate UserMessageTemplate { get; set; }
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var obj = (Message)item;
        if (obj.SenderId != AuthFields.UserId) return SenderMessageTemplate;
        
        return UserMessageTemplate;
    }
}
