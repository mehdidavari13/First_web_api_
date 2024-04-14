using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FirstWebAPI.Core.Dto.InvoiceDto;
using FirstWebAPI.Core.Dto.InvoiceItemDto;
using FirstWebAPI.Core.Dto.ProductDto;
using FirstWebAPI.Core.Entities;

namespace FirstWebAPI.Core.Configurations
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Product, GetProductDto>();
            CreateMap<AddProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
          
            CreateMap<InvoiceItem, GetInvoiceItemDto>();
            CreateMap<AddInvoiceItemDto, InvoiceItem>();
            CreateMap<UpdateInvoiceItemDto, InvoiceItem>();

            CreateMap<Invoice,GetInvoiceDto >();
            CreateMap<AddInvoiceDto, Invoice>();
            CreateMap<UpdateInvoiceDto, Invoice>();


            CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User!.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User!.LastName))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User!.Address))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User!.Phone))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User!.Email))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.InvoiceItems))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            CreateMap<InvoiceItem, ItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.UnitPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
}
