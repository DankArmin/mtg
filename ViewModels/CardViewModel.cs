using mtg_lib.Library.Models;

namespace mtg.ViewModels
{
    public class CardViewModel
    {
                public CardViewModel(Card card)
                {
                        this.Id = card.Id;
                        this.Name = card.Name;
                        this.ManaCost = card.ManaCost;
                        this.ConvertedManaCost = card.ConvertedManaCost;
                        this.Type = card.Type;
                        this.RarityCode = card.RarityCode;
                        this.SetCode = card.SetCode;
                        this.Text = card.Text;
                        this.Flavor = card.Flavor;
                        this.ArtistId = card.ArtistId;
                        this.Number = card.Number;
                        this.Power = card.Power;
                        this.Toughness = card.Toughness;
                        this.Layout = card.Layout;
                        this.MultiverseId = card.MultiverseId;
                        this.OriginalImageUrl = card.OriginalImageUrl;
                        this.Image = card.Image;
                        this.OriginalText = card.OriginalText;
                        this.OriginalType = card.OriginalType;
                }

        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ManaCost { get; set; }

        public string ConvertedManaCost { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string? RarityCode { get; set; }

        public string SetCode { get; set; } = null!;

        public string? Text { get; set; }

        public string? Flavor { get; set; }

        public long? ArtistId { get; set; }

        public string Number { get; set; } = null!;

        public string? Power { get; set; }

        public string? Toughness { get; set; }

        public string Layout { get; set; } = null!;

        public int? MultiverseId { get; set; }

        public string? OriginalImageUrl { get; set; }

        public string Image { get; set; } = null!;

        public string? OriginalText { get; set; }

        public string? OriginalType { get; set; }
    }
}