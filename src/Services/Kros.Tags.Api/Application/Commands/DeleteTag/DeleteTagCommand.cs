using MediatR;
using Newtonsoft.Json;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Delete tag command.
    /// </summary>
    public class DeleteTagCommand : IRequest<Unit>, ITagManagementCommand, IUserResourceCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagId">Tag id.</param>
        /// <param name="organizationId">Organization Id.</param>
        /// <param name="userId">User Id.</param>
        /// <param name="colorValue">ARGB value of color.</param>
        public DeleteTagCommand(long tagId, long organizationId, long userId, int colorValue)
        {
            Id = tagId;
            OrganizationId = organizationId;
            UserId = userId;
            ColorARGBValue = colorValue;
        }

        /// <summary>
        /// Id.
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Organization Id.
        /// </summary>
        [JsonIgnore]
        public long OrganizationId { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public long UserId { get; set; }

        /// <summary>
        /// ARGB value for color.
        /// </summary>
        public int ColorARGBValue { get; set; }
    }
}
