using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistant_specialist.Models.PositiveResponseM
{
    enum Discounts
    {
        [DescriptionValue("Скидка 50 руб.", Value = "50 (Пятьдесят) рублей 00 копеек")]
        RUB50,
        [DescriptionValue("Скидка 100 руб.", Value = "100 (Сто) рублей 00 копеек")]
        RUB100,
        [DescriptionValue("Скидка 150 руб.", Value = "150 (Сто пятьдесят) рублей 00 копеек")]
        RUB150,
        [DescriptionValue("Скидка 200 руб.", Value = "200 (Двести) рублей 00 копеек")]
        RUB200,
        [DescriptionValue("Скидка 250 руб.", Value = "250 (Двести пятьдесят) рублей 00 копеек")]
        RUB250,
        [DescriptionValue("Скидка 300 руб.", Value = "300 (Триста) рублей 00 копеек")]
        RUB300,
        [DescriptionValue("Скидка 350 руб.", Value = "350 (Триста пятьдесят) рублей 00 копеек")]
        RUB350
    }

    enum DiscountType
    {
        [DescriptionValue("Скидка")]
        Discount,
        [DescriptionValue("Перерасчёт")]
        Recalculation,
        [DescriptionValue("Скидка и перерасчёт")]
        DiscountAndRecalculation
    }

    enum Gender
    {
        [DescriptionValue("Мужской")]
        Male,
        [DescriptionValue("Женский")]
        Female
    }

    class PositiveResponseModel
    {

        #region Конструктор

        private PositiveResponseModel() { }

        #endregion

        #region Cоздать пустой документ

        public static PositiveResponseModel CreateNewDocument()
        {
            return new PositiveResponseModel()
            {
                Gender = Gender.Male,
                StatementRecalculation = true,
                DiscountType = DiscountType.Discount,
                Discounts = Discounts.RUB50
            };
        }

        #endregion

        #region Свойства документа

        //Номер договора
        public string ContractNumber { get; set; }

        //Дата договора
        public DateTime? ContractDate { get; set; }

        //Ф.И.О.
        public string FullName { get; set; }

        //Пол
        public Gender Gender { get; set; }

        //Адрес
        public string Address { get; set; }

        //Исходящий номер
        public string OutgoingNumber { get; set; }

        //Дата ответа
        public DateTime? OutgoingDate { get; set; }

        //Заявление на расторжение договора
        public bool StatementTermination { get; set; }

        //Дата расторжения договора
        public DateTime? ContractTerminationDate { get; set; }

        //Сумма долга
        public decimal AmountDebt { get; set; }

        //Заявление на перерасчет
        public bool StatementRecalculation { get; set; }

        //Дата заявления на перерасчет
        public DateTime? StatementRecalculationDate { get; set; }

        //Тип скидки
        public DiscountType DiscountType { get; set; }

        //Список скидок
        public Discounts Discounts { get; set; }

        //Дата начала скидки
        public DateTime? StartDateDiscount { get; set; }

        //Дата окончания скидки
        public DateTime? EndDateDiscount { get; set; }

        //Сумма перерасчета
        public decimal AmountRecalculation { get; set; }

        #endregion

    }
}