using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace drs_backend_phase1.Models.DTOs
{
    public class MockTestData
    {
        public IList<Profile> FakeProfile()
        {
            List<Profile> fakeProf = new List<Profile>
            {
                new Profile {
                    address1 = "1 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1960,1,1),
                    firstName ="Fred",
                    lastName = "Bloggs",
                    homeEmail = "me@me.com",
                    postcode = "PO5 T00",
                    id = 1
                },
                new Profile {
                    address1 = "2 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1961,1,1),
                    firstName ="Fred",
                    lastName = "Bloggs",
                    homeEmail = "him@me.com",
                    postcode = "PO5 T01",
                    id = 2
                },
                new Profile {
                    address1 = "3 Fake Street",
                    address2 = "FakeVille",
                    address4 = "FakeShire",
                    dateCreated = DateTime.Now,
                    dateOfBirth = new DateTime(1962,1,1),
                    firstName ="Fred",
                    lastName = "Bloggs",
                    homeEmail = "her@me.com",
                    postcode = "PO5 T02",
                    id = 3
                }
            };
            return fakeProf;
        }

        public IList<ProfileDocument> fakeDocuments()
        {
            List<ProfileDocument> fakeDoc = new List<ProfileDocument>
            {
                new ProfileDocument
                {
                    id=1,
                    dateCreated = new DateTime(2017,07,01),
                    dateExpires = new DateTime(2018,6,30),
                    dateModified = new DateTime(2017,7,1),
                    dateObtained = new DateTime(2017,6,6),
                    DocumentType = 1,
                    documentTypeId = 1,
                    originalFileName="OriginalFileName.one",
                    profileId = 1
                }
            };
            return fakeDoc;
        }

        public DocumentType FakeDocumentType()
        {
            DocumentType fakeDT = new DocumentType
            {
                id = 1,
                name = "Document Type One",
                dateCreated = new DateTime(2017, 6, 22),
            };
         return fakeDT;
        }
    }
}