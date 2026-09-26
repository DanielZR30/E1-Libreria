using Books.Application.UseCases.Books.Queries.GetBookById;
using Books.Application.UseCases.Books.Queries.GetBooksByCategory;
using Books.Application.UseCases.Books.Queries.GetBooksList;
using Books.Application.Utilities.Mediator;
using Books.Application.Utilities.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediator, SimpleMediator>();

            // Use Cases
            services.AddScoped<IRequestHandler<GetBooksListQuery, PaginationResponse<BookListItemDTO>>, GetBooksListUseCase>();
            services.AddScoped<IRequestHandler<GetBookByIdQuery, BookDetailsDTO?>, GetBookByIdUseCase>();
            services.AddScoped<IRequestHandler<GetBooksByCategoryQuery, PaginationResponse<BookListItemDTO>>, GetBooksByCategoryUseCase>();

            return services;
        }
    }
}
