using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirstWebAPI.Core.Contracts;
using FirstWebAPI.Core.Dto.InvoiceDto;
using FirstWebAPI.Core.Dto.InvoiceItemDto;
using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public InvoiceController(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddInvoice(AddInvoiceDto addInvoice)
        {
            var invoice = _mapper.Map<Invoice>(addInvoice);
            var response = await _invoiceRepository.AddAsync(invoice);
            return Ok(response);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var invoice = await _invoiceRepository.GetAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            return Ok(invoiceDto);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetInvoice()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            return Ok(invoices);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvoice(int id, UpdateInvoiceDto updateInvoice)
        {
            if (updateInvoice == null)
            {
                return BadRequest("the update Invoice is null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }
            var mainInvoice = await _invoiceRepository.GetAsync(id); 
            if (mainInvoice == null) 
            {
                return NotFound();
            }
           
            _mapper.Map(updateInvoice,mainInvoice);
            var responce = _invoiceRepository.UpdateAsync(mainInvoice);
            return Ok(responce);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
            var invoice = await _invoiceRepository.GetAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            else
            {
                await _invoiceRepository.DeleteAsync(id);
                return NoContent();
            }

        }
    }
}
