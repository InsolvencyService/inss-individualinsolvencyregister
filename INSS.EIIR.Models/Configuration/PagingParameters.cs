namespace INSS.EIIR.Models.Configuration;

public class PagingParameters
{
    const int maxPageSize = 1000;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }

    public int Skip
    {
        get 
        {
           return (PageNumber - 1) * PageSize;
        
        }
    }
}
