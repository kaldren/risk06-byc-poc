namespace OnboardingFunction.Services;
internal interface IOnboardingService
{
    Task StartOnboardingAsync(Manifest manifest);
}
