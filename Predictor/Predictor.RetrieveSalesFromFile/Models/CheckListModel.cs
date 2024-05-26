namespace Predictor.RetrieveSalesApi.Models;
// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class CheckItem
{
    public int id { get; set; }
    public int menu_item_id { get; set; }
    public string menu_item { get; set; }
    public string guest_check_name { get; set; }
    public int report_group_id { get; set; }
    public string sort_key { get; set; }
    public int station_id { get; set; }
    public string owner_timecard_id { get; set; }
    public int owner_id { get; set; }
    public string owner_name { get; set; }
    public DateTime time_ordered { get; set; }
    public int price_number { get; set; }
    public double quantity { get; set; }
    public double price { get; set; }
    public double extension { get; set; }
    public Flags flags { get; set; }
    public List<int> taxes_applied { get; set; }
    public int? parent_id { get; set; }
    public int? level { get; set; }
    public string approved_by { get; set; }
    public string discount_internal_id { get; set; }
    public double? discount_total { get; set; }
    public int? void_id { get; set; }
    public string void_name { get; set; }
    public int? voided_by_id { get; set; }
    public DateTime? time_voided { get; set; }
}

public class Discount
{
    public int id { get; set; }
    public int discount_id { get; set; }
    public string discount_id_explicit { get; set; }
    public string discount_internal_id { get; set; }
    public double amount { get; set; }
    public DateTime time_discounted { get; set; }
    public int approved_by_id { get; set; }
    public string approved_by_name { get; set; }
}

public class Flags
{
    public bool used { get; set; }
    public bool tab_name_entered { get; set; }
    public bool? reopened { get; set; }
    public bool print_modifier { get; set; }
    public bool? include_in_price { get; set; }
    public bool? kitchen_comment { get; set; }
    public bool? discounted { get; set; }
    public bool? extra { get; set; }
    public bool? no { get; set; }
    public bool? sub { get; set; }
    public bool? voided { get; set; }
    public bool verified { get; set; }
    public bool approved { get; set; }
    public bool? track2_recorded { get; set; }
    public bool flagsRedemption { get; set; }
}

public class GiftCard
{
    public int id { get; set; }
    public int revenue_center_id { get; set; }
    public int station_id { get; set; }
    public int owner_id { get; set; }
    public string owner_name { get; set; }
    public string owner_timecard_id { get; set; }
    public string account { get; set; }
    public double amount { get; set; }
    public DateTime timestamp { get; set; }
    public string approval { get; set; }
    public string reference_number { get; set; }
    public double remaining_balance { get; set; }
    public Flags flags { get; set; }
}

public class Payment
{
    public int id { get; set; }
    public int revenue_center_id { get; set; }
    public int payment_id { get; set; }
    public string payment_name { get; set; }
    public string account { get; set; }
    public string expiration_date { get; set; }
    public double amount { get; set; }
    public string approval { get; set; }
    public int owner_id { get; set; }
    public string owner_name { get; set; }
    public string owner_timecard_id { get; set; }
    public int station_id { get; set; }
    public DateTime timestamp { get; set; }
    public Flags flags { get; set; }
    public string customer_name { get; set; }
    public double? change_back { get; set; }
    public int? void_id { get; set; }
    public int? voided_by_id { get; set; }
    public string voided_by_name { get; set; }
    public DateTime? time_voided { get; set; }
}

public class Root
{
    public string id { get; set; }
    public string table { get; set; }
    public int guests { get; set; }
    public int order_type_id { get; set; }
    public int revenue_center_id { get; set; }
    public string revenue_center_name { get; set; }
    public int owner_id { get; set; }
    public string owner_name { get; set; }
    public string owner_timecard_id { get; set; }
    public DateTime time_opened { get; set; }
    public int opener_id { get; set; }
    public string opener_name { get; set; }
    public int opened_station_id { get; set; }
    public DateTime time_closed { get; set; }
    public int closer_id { get; set; }
    public string closer_name { get; set; }
    public string closer_timecard_id { get; set; }
    public int closed_station_id { get; set; }
    public Flags flags { get; set; }
    public double total { get; set; }
    public List<TaxableSale> taxable_sales { get; set; }
    public List<object> tax_exempt_sales { get; set; }
    public List<Seat> seats { get; set; }
    public double? discount_total { get; set; }
    public double? void_total { get; set; }
    public DateTime? time_reopened { get; set; }
}

public class Seat
{
    public int id { get; set; }
    public int seat_number { get; set; }
    public Flags flags { get; set; }
    public int times_printed { get; set; }
    public double subtotal { get; set; }
    public double food_total { get; set; }
    public double total { get; set; }
    public List<Taxis> taxes { get; set; }
    public List<CheckItem> check_items { get; set; }
    public List<Payment> payments { get; set; }
    public List<GiftCard> gift_cards { get; set; }
    public List<Discount> discounts { get; set; }
    public double? change_back { get; set; }
    public double? item_discount_total { get; set; }
}

public class TaxableSale
{
    public int id { get; set; }
    public double amount { get; set; }
}

public class Taxis
{
    public int id { get; set; }
    public double amount { get; set; }
}
