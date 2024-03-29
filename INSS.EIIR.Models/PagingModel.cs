﻿namespace INSS.EIIR.Models;

public class PagingModel
{
    public PagingModel(int resultCount, int page, int pageSize = 10)
    {
        ResultCount = resultCount;
        Page = page;
        TotalPages = resultCount == 0 ? 0 : (int)Math.Ceiling((double)resultCount / pageSize);
    }

    public string Parameters { get; set; }

    public int ResultCount { get; set; }

    public int Page { get; set; }

    public int TotalPages { get; set; }

    public string RootUrl { get; set; }

    public string SearchTerm { get; set; }
}
