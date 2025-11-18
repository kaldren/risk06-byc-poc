namespace OnboardingFunction.Services;
public interface IOnboardingService
{
    Task StartOnboardingAsync(Manifest manifest = null);
}
