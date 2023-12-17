public interface IGhost
{
    bool IsGhost { get; set; }
    
    void Ghost()
    {
        IsGhost = true;
    }
    void UnGhost()
    {
        IsGhost = false;
    }
}