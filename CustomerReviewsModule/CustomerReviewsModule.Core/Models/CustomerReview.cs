﻿using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviewsModule.Core.Models
{
    public class CustomerReview : AuditableEntity
    {
        public string AuthorNickname { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public string ProductId { get; set; }
        public int Rating { get; set; }
    }
}
