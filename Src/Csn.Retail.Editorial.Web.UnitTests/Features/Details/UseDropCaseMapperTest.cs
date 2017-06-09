using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Details.Mappings;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Details
{
    [TestFixture]
    internal class UseDropCaseMapperTest
    {
        [Test]
        public void WithContentValue()
        {
            var article = new ArticleDetailsDto
            {
                ContentSections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        SectionType = ContentSectionType.Html,
                        Content = "<p>Backstage pass to the biggest one-make automotive party of the year and the smorgasbord of stars on this red carpet will have you salivating. Alongside me holding up the score cards are senior motoring.com.au scribes Mike Sinclair, Marton Pettendy and Andrea Matthews, providing a good cross-section of experience and input. Truth be known, they're also there to make sure I don't spend two days just lapping in the M4 GTS</p>"
                    },
                    new ContentSection
                    {
                        SectionType = ContentSectionType.SingleImage,
                        Image = new Image
                        {
                            Url = "https://editorial.li.csnstatic.com/carsales/general/editorial/161220_BMW_M5_02.jpg"
                        }
                    }
                }
            };

            var useCaseDropMapper = new UseDropCaseMapper();

            var result = useCaseDropMapper.Map(article);

            Assert.IsTrue(result);
        }


        [Test]
        public void WithoutContentValue()
        {
            var article = new ArticleDetailsDto
            {
                ContentSections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        SectionType = ContentSectionType.Html,
                    },
                    new ContentSection
                    {
                        SectionType = ContentSectionType.SingleImage,
                        Image = new Image
                        {
                            Url = "https://editorial.li.csnstatic.com/carsales/general/editorial/161220_BMW_M5_02.jpg"
                        }
                    }
                }
            };

            var useCaseDropMapper = new UseDropCaseMapper();

            var result = useCaseDropMapper.Map(article);

            Assert.IsFalse(result);
        }

        [Test]
        public void WithShortContentValue()
        {
            var article = new ArticleDetailsDto
            {
                ContentSections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        SectionType = ContentSectionType.Html,
                        Content = "<p>Backstage pass to the biggest one-make automotive party of the year."
                    },
                    new ContentSection
                    {
                        SectionType = ContentSectionType.SingleImage,
                        Image = new Image
                        {
                            Url = "https://editorial.li.csnstatic.com/carsales/general/editorial/161220_BMW_M5_02.jpg"
                        }
                    }
                }
            };

            var useCaseDropMapper = new UseDropCaseMapper();

            var result = useCaseDropMapper.Map(article);

            Assert.IsFalse(result);
        }

        [Test]
        public void WithNumericBeginningContentValue()
        {
            var article = new ArticleDetailsDto
            {
                ContentSections = new List<ContentSection>
                {
                    new ContentSection
                    {
                        SectionType = ContentSectionType.Html,
                        Content = "<p>M310 backstage pass to the biggest one-make automotive party of the year."
                    },
                    new ContentSection
                    {
                        SectionType = ContentSectionType.SingleImage,
                        Image = new Image
                        {
                            Url = "https://editorial.li.csnstatic.com/carsales/general/editorial/161220_BMW_M5_02.jpg"
                        }
                    }
                }
            };

            var useCaseDropMapper = new UseDropCaseMapper();

            var result = useCaseDropMapper.Map(article);

            Assert.IsFalse(result);
        }
    }
}
