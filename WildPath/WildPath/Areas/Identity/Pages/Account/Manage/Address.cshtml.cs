using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using WildPath.EfModels;
using WildPath.Repositories;

public class AddressModel : PageModel
{
    private readonly MyRegisteredUserRepo _userRepo;
    // Assume UserManager is injected and available to get the current user

    public Address MailingAddress { get; set; }
    public Address ShippingAddress { get; set; }

    public AddressModel(MyRegisteredUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public class AddressModelInput
    {
        public int? PkAddressId { get; set; }
        public int? Unit { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
    }

    [BindProperty]
    public AddressModelInput MailingAddressInput { get; set; } = new AddressModelInput();

    [BindProperty]
    public AddressModelInput ShippingAddressInput { get; set; } = new AddressModelInput();

    public async Task<IActionResult> OnGetAsync()
    {
        var email = User.Identity.Name;
        var user = _userRepo.GetUserByEmail(email);

        MailingAddressInput = new AddressModelInput
        {
            PkAddressId = user.FkMailingAdress?.PkAddressId,
            Unit = user.FkMailingAdress?.Unit,
            Address1 = user.FkMailingAdress?.Address1,
            City = user.FkMailingAdress?.City,
            Province = user.FkMailingAdress?.Province,
            PostalCode = user.FkMailingAdress?.PostalCode
        };

        ShippingAddressInput = new AddressModelInput
        {
            PkAddressId = user.FkShippingAdress?.PkAddressId,
            Unit = user.FkShippingAdress?.Unit,
            Address1 = user.FkShippingAdress?.Address1,
            City = user.FkShippingAdress?.City,
            Province = user.FkShippingAdress?.Province,
            PostalCode = user.FkShippingAdress?.PostalCode
        };

        return Page();
    }


    public async Task<IActionResult> OnPostMailingAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = _userRepo.GetUserByEmail(User.Identity.Name);

        var mailingAddress = new Address
        {
            PkAddressId = MailingAddressInput.PkAddressId ?? 0,
            Unit = MailingAddressInput.Unit,
            Address1 = MailingAddressInput.Address1,
            City = MailingAddressInput.City,
            Province = MailingAddressInput.Province,
            PostalCode = MailingAddressInput.PostalCode
        };

        var updatedAddress = _userRepo.AddOrUpdateAddress(mailingAddress);

        user.FkMailingAdressId = updatedAddress.PkAddressId;
        _userRepo.UpdateUser(user);

        return RedirectToPage(new { successMessage = "Mailing address updated successfully." });
    }

    public async Task<IActionResult> OnPostShippingAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = _userRepo.GetUserByEmail(User.Identity.Name);

        var shippingAddress = new Address
        {
            PkAddressId = ShippingAddressInput.PkAddressId ?? 0,
            Unit = ShippingAddressInput.Unit,
            Address1 = ShippingAddressInput.Address1,
            City = ShippingAddressInput.City,
            Province = ShippingAddressInput.Province,
            PostalCode = ShippingAddressInput.PostalCode
        };

        var updatedAddress = _userRepo.AddOrUpdateAddress(shippingAddress);

        user.FkShippingAdressId = updatedAddress.PkAddressId;
        _userRepo.UpdateUser(user);

        return RedirectToPage(new { successMessage = "Shipping address updated successfully." });
    }

}
