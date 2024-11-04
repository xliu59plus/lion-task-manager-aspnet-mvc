public class ProductEntity
{
    public int Id {get; set;} = 0;
    public string Product {get; set;} = string.Empty;
    public long Rate {get; set;} =  long.MaxValue;
    public long Quantity {get; set;} = long.MaxValue;
    public int MyProperty {get; set;} = int.MaxValue;
    //public string ImagePath {get; set;}
}