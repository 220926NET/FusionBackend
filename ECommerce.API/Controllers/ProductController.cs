﻿using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService service, ILogger<ProductController> logger)
        {
            this._service = service;
            this._logger = logger;
        }


        [HttpGet("{productName}")]
        public async Task<ActionResult<Product>> GetOne(string productName)
        {
            _logger.LogInformation("api/product/{productName} triggered");
            try
            {
                _logger.LogInformation("api/product/{productName} completed successfully");
                Product product = await _service.GetProductByNameAsync(productName);
                if (product.name != null)
                {
                    return Ok(product);
                }
                else
                {
                    return BadRequest("Invalid ID");
                }
            }
            catch
            {
                _logger.LogWarning("api/product/{id} completed with errors");
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult<List<Product>> GetAll()
        {
            _logger.LogInformation("api/product triggered");
            try
            {
                _logger.LogInformation("api/product completed successfully");
                return Ok(_service.GetAllProducts());
            }
            catch
            {
                _logger.LogWarning("api/product completed with errors");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("sale")]
        public ActionResult<List<Product>> GetAllSale()
        {
            _logger.LogInformation("api/product triggered");
            try
            {
                _logger.LogInformation("api/product completed successfully");
                return Ok(_service.GetSaleProducts());
            }
            catch
            {
                _logger.LogWarning("api/product completed with errors");
                return BadRequest();
            }
        }

        [HttpPatch]
        public async Task<ActionResult<Product[]>> Purchase([FromBody] ProductDTO[] purchaseProducts)
        {
            _logger.LogInformation("PATCH api/product triggered");
            List<Product> products = new List<Product>();
            try
            {
                foreach (ProductDTO item in purchaseProducts)
                {
                    Product tmp = await _service.GetProductByIdAsync(item.id);
                    if ((tmp.quantity - item.quantity) >= 0)
                    {
                        await _service.ReduceInventoryByIdAsync(item.id, item.quantity);
                        products.Add(await _service.GetProductByIdAsync(item.id));
                    }
                    else
                    {
                        throw new Exception("Insufficient inventory.");
                    }
                }
                _logger.LogInformation("PATCH api/product completed successfully");
                return Ok(products);
            }
            catch
            {
                _logger.LogWarning("PATCH api/product completed with errors");
                return BadRequest();

            }


        }
    }
}
