using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "Order")]
    public class Order
    {
        [XmlElement(ElementName = "Item")]
        public List<Item>? Item { get; set; }
        [XmlAttribute(AttributeName = "saleType")]
        public string? SaleType { get; set; }
        [XmlAttribute(AttributeName = "totalAmount")]
        public string? TotalAmount { get; set; }
        [XmlAttribute(AttributeName = "totalTax")]
        public string? TotalTax { get; set; }
        [XmlAttribute(AttributeName = "startSaleDate")]
        public string? StartSaleDate { get; set; }
        [XmlAttribute(AttributeName = "startSaleTime")]
        public string? StartSaleTime { get; set; }
    }

}
