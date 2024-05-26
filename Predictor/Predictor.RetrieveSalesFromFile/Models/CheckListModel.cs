namespace Predictor.RetrieveSalesApi.Models;

public class CheckListModel
{
    public Class1[] Property1 { get; set; }
}

public class Class1
{
    public int id { get; set; }
    public string table { get; set; }
    public string guests { get; set; }
    public int order_type_id { get; set; }
    public int revenue_center_id { get; set; }
    public string revenue_center_name { get; set; }
    public int location_id { get; set; }
    public string customer_id { get; set; }
    public int owner_id { get; set; }
    public string owner_name { get; set; }
    public string owner_timecard_id { get; set; }
    public string time_opened { get; set; }
    public int opener_id { get; set; }
    public string opener_name { get; set; }
    public int opened_station_id { get; set; }
    public string opened_station_name { get; set; }
    public string time_closed { get; set; }
    public int closer_id { get; set; }
    public string closer_name { get; set; }
    public string closer_timecard_id { get; set; }
    public int closer_station { get; set; }
    public string closed_station_name { get; set; }
    public Flags flags { get; set; }
    public string time_reopened { get; set; }
    public string time_transfered { get; set; }
    public int transfer_from_id { get; set; }
    public string transfer_from_name { get; set; }
    public int transfer_to_id { get; set; }
    public int transfer_to_idcopy { get; set; }
    public int total { get; set; }
    public int void_total { get; set; }
    public int discount_total { get; set; }
    public int order_type_charge_item_discount { get; set; }
    public int giftcard_item_discount { get; set; }
    public bool all_seats_voided { get; set; }
    public Taxable_Sales[] taxable_sales { get; set; }
    public string tax_exempt_id { get; set; }
    public Tax_Exempt_Sales[] tax_exempt_sales { get; set; }
    public Seat[] seats { get; set; }
}

public class Flags
{
    public bool used { get; set; }
    public bool open { get; set; }
    public bool tab_name_entered { get; set; }
    public bool reopened { get; set; }
    public bool transferred { get; set; }
    public bool renumbered { get; set; }
    public bool moved { get; set; }
    public bool refund { get; set; }
    public bool beverages_ordered { get; set; }
    public bool appetizers_ordered { get; set; }
    public bool entress_ordered { get; set; }
    public bool desserts_ordered { get; set; }
    public bool missing_beverage { get; set; }
}

public class Taxable_Sales
{
    public int id { get; set; }
    public int amount { get; set; }
}

public class Tax_Exempt_Sales
{
    public int id { get; set; }
    public int amount { get; set; }
}

public class Seat
{
    public int id { get; set; }
    public int seat_number { get; set; }
    public Flags1 flags { get; set; }
    public int times_printed { get; set; }
    public int subtotal { get; set; }
    public int item_discount_total { get; set; }
    public int subtotal_discount_total { get; set; }
    public int order_type_charge_total { get; set; }
    public int gratuity_id { get; set; }
    public int gratuity_total { get; set; }
    public int advance_total { get; set; }
    public int gift_card_total { get; set; }
    public int credit_card_surcharge_total { get; set; }
    public int change_back { get; set; }
    public int food_total { get; set; }
    public int beverage_total { get; set; }
    public int other_total { get; set; }
    public int total { get; set; }
    public Taxes taxes { get; set; }
    public Check_Items[] check_items { get; set; }
    public Payment[] payments { get; set; }
    public Gift_Cards[] gift_cards { get; set; }
    public Discount[] discounts { get; set; }
}

public class Flags1
{
    public bool used { get; set; }
    public bool open { get; set; }
}

public class Taxes
{
    public bool used { get; set; }
    public bool open { get; set; }
}

public class Check_Items
{
    public int id { get; set; }
    public int parent_id { get; set; }
    public int menu_item_id { get; set; }
    public string menu_item { get; set; }
    public string guest_check_name { get; set; }
    public int report_group_id { get; set; }
    public string sort_key { get; set; }
    public string station_id { get; set; }
    public string owner_id { get; set; }
    public string owner_timecard_id { get; set; }
    public string owner_name { get; set; }
    public string time_ordered { get; set; }
    public int level { get; set; }
    public int price_number { get; set; }
    public string price_number_name { get; set; }
    public int quantity { get; set; }
    public int modifier_quantity { get; set; }
    public int price { get; set; }
    public int extension { get; set; }
    public Flags2 flags { get; set; }
    public string approved_by_name { get; set; }
    public int void_id { get; set; }
    public string void_name { get; set; }
    public int voided_by_id { get; set; }
    public string time_voided { get; set; }
    public string time_held { get; set; }
    public int order_type_total { get; set; }
    public string discount_internal_id { get; set; }
    public int discount_total { get; set; }
    public int[] taxes_applied { get; set; }
}

public class Flags2
{
    public bool _void { get; set; }
    public bool discounted { get; set; }
    public bool sub { get; set; }
    public bool extra { get; set; }
    public bool no { get; set; }
    public bool include_in_price { get; set; }
    public bool print_modifier { get; set; }
    public bool kitchen_comment { get; set; }
    public bool discount_requirement { get; set; }
    public bool timed_rate { get; set; }
    public bool return_item { get; set; }
}

public class Payment
{
    public int id { get; set; }
    public int revenue_center_id { get; set; }
    public int payment_id { get; set; }
    public string payment_name { get; set; }
    public string customer_name { get; set; }
    public string local_customer_account { get; set; }
    public string post_account { get; set; }
    public string account { get; set; }
    public string expiration_date { get; set; }
    public int amount { get; set; }
    public int tip { get; set; }
    public int tip_fee { get; set; }
    public string approval { get; set; }
    public int change_back { get; set; }
    public int owner_id { get; set; }
    public string owner_name { get; set; }
    public string owner_timecard_id { get; set; }
    public int station_id { get; set; }
    public string timestamp { get; set; }
    public Flags3 flags { get; set; }
    public int void_id { get; set; }
    public int voided_by_id { get; set; }
    public string voided_by_name { get; set; }
    public string time_voided { get; set; }
    public string adjustor { get; set; }
    public int pre_adjustment { get; set; }
    public int pre_adjusted_amount { get; set; }
    public string time_adjusted { get; set; }
    public int check_number { get; set; }
    public int aba_routing { get; set; }
    public string drivers_license_number { get; set; }
    public string drivers_license_state { get; set; }
    public string birth_date { get; set; }
    public string time_drawer_closed { get; set; }
}

public class Flags3
{
    public bool voided { get; set; }
    public bool verified { get; set; }
    public bool pending { get; set; }
    public bool approved { get; set; }
    public bool preauthed { get; set; }
    public bool track2_recorded { get; set; }
    public bool adjusted { get; set; }
    public bool refunded { get; set; }
}

public class Gift_Cards
{
    public string id { get; set; }
    public int revenue_center_id { get; set; }
    public int station_id { get; set; }
    public int owner_id { get; set; }
    public string owner_name { get; set; }
    public int owner_timecard_id { get; set; }
    public string account { get; set; }
    public int amount { get; set; }
    public string timestamp { get; set; }
    public string approval { get; set; }
    public string reference_number { get; set; }
    public int remaining_balance { get; set; }
    public Flags4 flags { get; set; }
    public int void_id { get; set; }
    public int voided_by_id { get; set; }
    public string voided_by_name { get; set; }
    public string time_voided { get; set; }
    public string discount_internal_id { get; set; }
    public int discount_amount { get; set; }
}

public class Flags4
{
    public bool redemption { get; set; }
    public bool activation { get; set; }
    public bool increment { get; set; }
}

public class Discount
{
    public string id { get; set; }
    public int discount_id { get; set; }
    public string discount_id_explicit { get; set; }
    public string discount_internal_id { get; set; }
    public string source { get; set; }
    public int amount { get; set; }
    public string time_discounted { get; set; }
    public int approved_by_id { get; set; }
    public string approved_by_name { get; set; }
    public Decrease_Tax_Exempt_Totals[] decrease_tax_exempt_totals { get; set; }
}

public class Decrease_Tax_Exempt_Totals
{
    public int id { get; set; }
    public int amount { get; set; }
}

