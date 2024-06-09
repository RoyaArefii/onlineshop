using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicTools.Constants
{
    public static class EndPointConstants
    {
        public const string ControllerRoute = "api/[Controller]";
    }
    public static class Product
    {
        public const string Post = $"Post{nameof(Product)}";
        public const string Put = $"Put{nameof(Product)}";
        public const string Delete = $"Delete{nameof(Product)}";
        public const string GetAll = $"GetAll{nameof(Product)}";
    }
    public static class OrderHeader
    {
        public const string Post = $"Post{nameof(OrderHeader)}";
        public const string Put = $"Put{nameof(OrderHeader)}";
        public const string Delete = $"Delete{nameof(OrderHeader)}";
        public const string GetAll = $"GetAll{nameof(OrderHeader)}";
        //public const string DeleteDetail = $"DeleteOrderDetail{nameof(OrderHeader)}";
    }
    public static class OrderDetail
    {
        public const string Post = $"Post{nameof(OrderDetail)}";
        public const string Put = $"Put{nameof(OrderDetail)}";
        public const string Delete = $"Delete{nameof(OrderDetail)}";
        public const string GetAll = $"GetAll{nameof(OrderDetail)}";
    }
    public static class ProductCategory
    {
        public const string Post = $"Post{nameof(ProductCategory)}";
        public const string Put = $"Put{nameof(ProductCategory)}";
        public const string Delete = $"Delete{nameof(ProductCategory)}";
        public const string GetAll = $"GetAll{nameof(ProductCategory)}";
    }
}