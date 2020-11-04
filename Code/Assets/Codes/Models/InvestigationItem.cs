using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InvestigationItem
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Desription { get; private set; }

    public InvestigationItem(int id, string title, string desription)
    {
        Id = id;
        Title = title;
        Desription = desription;
    }
}

