namespace WebSmartKid.Models.Entity
{
    public class ReportQty
    {                                       
        public string Name { get; set; }     
        public decimal Price { get; set; }        
        public int QtyCurrent  { get; set; }     
        public decimal SumPriceQtyCurrent  { get; set; }     
        public int QtySales  { get; set; }     
        public decimal SumPriceQtySales  { get; set; }     
        public string CategoriesName { get; set; }
        public int TotalQtyCurrent { get; set; }
        public int TotalQtySales { get; set; }
        public decimal TotalSumPriceQtyCurrent { get; set; }
        public decimal TotalSumPriceQtySales { get; set; }
    }
}
