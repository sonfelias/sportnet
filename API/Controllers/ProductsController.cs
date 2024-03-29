using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    //[ApiController]
    // [Route("api/[controller]")]
    //public class ProductsController : ControllerBase
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo,
                                  IGenericRepository<ProductBrand> productBrandRepo,
                                  IGenericRepository<ProductType> productTypeRepo,
                                  IMapper mapper  )
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        //public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts(){
        // public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(
        //         string sort, int? brandId, int? typeId)
        // public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(
        //         [FromQuery]ProductSpecParams productParams)
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
                [FromQuery]ProductSpecParams productParams)
        {
            //var spec = new ProductsWithTypesAndBrandsSpecification(sort, brandId, typeId);
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productRepo.CountAsync(countSpec);

            var products = await _productRepo.ListAsync(spec);
            //return Ok(products);

            // var productsToReturnDto = products.Select(product => new ProductToReturnDto{
            //     Id = product.Id,
            //     Name  = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // }).ToList();
            // return Ok(productsToReturnDto);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            //return Ok( _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, 
                                                            totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id){
            // var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            // return Ok(product);
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            //return Ok(await _productRepo.GetByIdAsync(id));
            //return Ok(await _productRepo.GetEntityWithSpec(spec));
            var product = await _productRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));

            // return Ok( new ProductToReturnDto {
            //     Id = product.Id,
            //     Name  = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // });

            

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            return Ok(await _productBrandRepo.ListAllAsync());            
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){
            return Ok(await _productTypeRepo.ListAllAsync());            
        }


    }
}