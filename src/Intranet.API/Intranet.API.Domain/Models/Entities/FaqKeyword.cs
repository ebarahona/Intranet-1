﻿using Intranet.API.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Intranet.API.Domain.Models.Entities
{
    public class FaqKeyword : KeywordRelation, IKeywordRelation
    {
        public FaqKeyword()
        {
            // Empty
        }

        public FaqKeyword(Keyword keyword)
            : base(keyword)
        {
            // Empty
        }

        public int FaqId { get; set; }
        public Faq Faq { get; set; }
    }
}
