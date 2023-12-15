interface IInvisible
{
    bool IsInvisible { get; set; }
    
    void Visible()
    {
        IsInvisible = false;
    }
    void Invisible()
    {
        IsInvisible = true;
    }
}