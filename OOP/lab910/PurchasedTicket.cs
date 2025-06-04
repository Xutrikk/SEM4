using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using lab4wpf5oop.Models;

public class PurchasedTicket : INotifyPropertyChanged
{
    private int _purchaseId;
    private string _emailUser;
    private int _ticketId;
    private string _from;
    private string _to;
    private DateTime _purchaseDate;
    private TimeSpan _purchaseTime;
    private double _price;
    private int _number;
    private int _status;
    private string _type;
    private string _boardingPoints;
    private string _dropOffPoints;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("PurchaseId")]
    public int PurchaseId
    {
        get => _purchaseId;
        set { _purchaseId = value; OnPropertyChanged(); }
    }

    [ForeignKey("User")]
    [Column("EmailUser")]
    public string EmailUser
    {
        get => _emailUser;
        set { _emailUser = value; OnPropertyChanged(); }
    }

    [ForeignKey("Ticket")]
    [Column("TicketId")]
    public int TicketId
    {
        get => _ticketId;
        set { _ticketId = value; OnPropertyChanged(); }
    }

    public virtual TripRating TripRating { get; set; }

    [Column("From")]
    public string From
    {
        get => _from;
        set { _from = value; OnPropertyChanged(); }
    }

    [Column("To")]
    public string To
    {
        get => _to;
        set { _to = value; OnPropertyChanged(); }
    }

    [Column("PurchaseDate")]
    public DateTime PurchaseDate
    {
        get => _purchaseDate;
        set { _purchaseDate = value; OnPropertyChanged(); }
    }

    [Column("PurchaseTime")]
    public TimeSpan PurchaseTime
    {
        get => _purchaseTime;
        set { _purchaseTime = value; OnPropertyChanged(); }
    }

    [Column("Price")]
    public double Price
    {
        get => _price;
        set { _price = value; OnPropertyChanged(); }
    }

    [Column("Number")]
    public int Number
    {
        get => _number;
        set { _number = value; OnPropertyChanged(); }
    }

    [Column("Status")]
    public int Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(StatusText));
        }
    }

    [Column("Type")]
    public string Type
    {
        get => _type;
        set { _type = value; OnPropertyChanged(); }
    }

    [Column("BoardingPoints")]
    public string BoardingPoints
    {
        get => _boardingPoints;
        set { _boardingPoints = value; OnPropertyChanged(); }
    }

    [Column("DropOffPoints")]
    public string DropOffPoints
    {
        get => _dropOffPoints;
        set { _dropOffPoints = value; OnPropertyChanged(); }
    }

    [NotMapped]
    public string StatusText => Status == 0 ? "Не оплачено" : "Оплачено";

    public User User { get; set; }
    public Ticket Ticket { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}