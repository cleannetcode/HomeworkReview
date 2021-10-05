﻿using LessonMonitor.Core;
using LessonMonitor.DataAccess;

namespace LessonMonitor.BusinessLogic
{
    public class ProductDetailsService : IProductDetailsService
    {
        private IRepository<ProductDetails> productDetailsRepository;

        public ProductDetailsService(IRepository<ProductDetails> productDetailsRepository)
        {
            this.productDetailsRepository = productDetailsRepository;
        }

        public ProductDetails GetProductDetail(int id)
        {
            return productDetailsRepository.Get(id);
        }
    }
}
