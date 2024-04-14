using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstWebAPI.Core.Contracts;
using FirstWebAPI.Core.Dto.InvoiceItemDto;
using FirstWebAPI.Core.Dto.ProductDto;
using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Controllers
{
    [Authorize]   
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceItemController : ControllerBase
    {
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public InvoiceItemController(IInvoiceItemRepository invoiceItemRepository, IInvoiceRepository invoiceRepository,IProductRepository productRepository, IMapper mapper)
        {
            _invoiceItemRepository = invoiceItemRepository;
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddInvoiceItem(AddInvoiceItemDto addInvoiceItem)
        {
            var isInvoicExist = await _invoiceRepository.Exists(addInvoiceItem.InvoiceId);
            var isProductExist = await _productRepository.Exists(addInvoiceItem.ProductId);
            if (isInvoicExist && isProductExist)
            {
                var invoiceItem = _mapper.Map<InvoiceItem>(addInvoiceItem);


                var invoiceItem1 = await _invoiceItemRepository.IsDuplicateItemProduct(invoiceItem);
                if (invoiceItem1 != null)
                {

                    invoiceItem1.Quantity = invoiceItem1.Quantity + invoiceItem.Quantity;
                    await _invoiceItemRepository.UpdateAsync(invoiceItem1);
                    await _invoiceRepository.UpdateTotalAmount(invoiceItem1.Invoice.Id);
                    return Ok(invoiceItem1);
                }
                else
                {
                    var response = await _invoiceItemRepository.AddAsync(invoiceItem);
                    await _invoiceRepository.UpdateTotalAmount(invoiceItem.Invoice.Id);
                    return Ok(Response);
                }
                
            }
            else
                return BadRequest("Invoice or Product dose not exist!!!");

        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var invoiceItem = await _invoiceItemRepository.GetAsync(id);
            if (invoiceItem == null)
            {
                return NotFound();
            }

            return Ok(invoiceItem);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetInvoiceItems()
        {
            var invoiceItems = await _invoiceItemRepository.GetAllAsync();
            return Ok(invoiceItems);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvoiceItem(int id, UpdateInvoiceItemDto updateInvoiceItem)
        {
            var invoiceItem = _mapper.Map<InvoiceItem>(updateInvoiceItem);
            var responce = _invoiceItemRepository.UpdateAsync(invoiceItem);
            await _invoiceRepository.UpdateTotalAmount(invoiceItem.Invoice.Id);
            return Ok(responce);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoiceItem(int id)
        {
            var invoiceItem = await _invoiceItemRepository.GetAsync(id);
            if (invoiceItem == null)
            {
                return NotFound();
            }
            else
            {
                await _invoiceItemRepository.DeleteAsync(id);
                return NoContent();
            }

        }
    }
}
