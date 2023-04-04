using NoteTakingApp.Models.DTO;

namespace NoteTakingApp.Repository.interfaces
{
    public interface IAuthRepository
    {

        AjaxResponse Login(LoginData loginData);
        AjaxResponse Registration(RegistartionData registerData);
        void Logout();

    }
}
