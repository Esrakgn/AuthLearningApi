using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AuthLearningApi.Application.Common.Models;

public class PagedResult<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<T> Items { get; set; } = new();
}

// toplam kaç kayıt var kaçı sunulacak bunu gösterir