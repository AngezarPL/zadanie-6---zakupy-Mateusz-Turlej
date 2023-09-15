using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using zadanie6.Data;
using zadanie6.Models;

namespace zadanie_6___zakupy.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly zadanie6Context context;

    public MainViewModel(zadanie6Context context)
    {
        this.context = context;
        Purchases = context.Purchases
            .LoadAsync()
            .ContinueWith(p => context.Purchases.Local.ToObservableCollection());

    }
    [RelayCommand]
    private async Task AddPurchaseAsync()
    {
        if (newPurchase.Validate())
        {
            context.Add(NewPurchase);
            await context.SaveChangesAsync();
            newPurchase = new Purchase();
        }
    }

    [RelayCommand]
    private async Task DeletePurchaseAsync(Purchase purchase)
    {
        context.Remove(purchase);
        await context.SaveChangesAsync();
    }

    [ObservableProperty]
    private Purchase newPurchase = new Purchase();

    private TaskNotifier<ObservableCollection<Purchase>> purchases;
    public Task<ObservableCollection<Purchase>> Purchases
    {
        get => purchases;
        set => SetPropertyAndNotifyOnCompletion(ref purchases, value);
    }
}
