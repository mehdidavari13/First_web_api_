using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using FirstWebAPI.Core.Contracts;
using FirstWebAPI.Core.Dto.ProductDto;
using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository ,IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(AddProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var response = await _productRepository.AddAsync(product);
            return Ok(response);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetProduct()
        {
            var Products = await _productRepository.GetAllAsync();
            return Ok(Products);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            var product = _mapper.Map<Product>(updateProductDto);
            var responce = _productRepository.UpdateAsync(product);
            return Ok(responce);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                await _productRepository.DeleteAsync(id);
                return NoContent();
            }

        }
    }
}
