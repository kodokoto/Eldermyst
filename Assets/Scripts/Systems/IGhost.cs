interface IGhost
{
    bool IsImmaterial { get; set; }
    void Immaterialize() 
    {
        IsImmaterial = true;
    }
    void Materialize()
    {
        IsImmaterial = false;
    }
}
