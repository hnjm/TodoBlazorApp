using AutoMapper;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Entities;

namespace TodoBlazorApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TodoItem mappings
        CreateMap<TodoItem, TodoItemDto>()
            .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue()));
        
        CreateMap<CreateTodoItemDto, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false));
        
        CreateMap<UpdateTodoItemDto, TodoItem>()
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom((src, dest) => 
                src.IsCompleted && !dest.IsCompleted ? DateTime.UtcNow : dest.CompletedDate));

        // TodoComment mappings
        CreateMap<TodoComment, TodoCommentDto>();
        CreateMap<CreateTodoCommentDto, TodoComment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // TodoAttachment mappings
        CreateMap<TodoAttachment, TodoAttachmentDto>();
    }
}