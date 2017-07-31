using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace drs_backend_phase1.Models.MockSetup
{
    public class FakeProfiles
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
    }
}