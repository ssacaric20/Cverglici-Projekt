using SmartMenza.Business.Models.FoodAnalysis;
using SmartMenza.Business.Services.Interfaces;

namespace SmartMenza.Business.Services
{
    public sealed class RuleBasedFoodAnalyzer : IFoodAnalyzer
    {
        private static readonly Dictionary<string, string[]> AllergenKeywords = new()
        {
            ["Cereals containing gluten"] = new[]
            {
            "wheat", "rye", "barley", "oats", "spelt", "kamut", "triticale",
            "malt", "semolina", "durum", "bulgur", "couscous", "seitan",
            "breadcrumbs", "wheat flour", "barley malt"
        },
            ["Crustaceans"] = new[]
            {
            "shrimp", "prawn", "crab", "lobster", "crayfish", "krill"
        },
            ["Eggs"] = new[]
            {
            "egg", "eggs", "albumen", "mayonnaise", "mayo"
        },
            ["Fish"] = new[]
            {
            "fish", "salmon", "tuna", "anchovy", "anchovies", "cod", "haddock",
            "sardine", "sardines", "mackerel"
        },
            ["Peanuts"] = new[]
            {
            "peanut", "peanuts", "groundnut", "groundnuts"
        },
            ["Soybeans"] = new[]
            {
            "soy", "soya", "tofu", "edamame", "miso", "tempeh", "soy sauce"
        },
            ["Milk"] = new[]
            {
            "milk", "butter", "cheese", "cream", "yogurt", "yoghurt", "whey",
            "casein", "lactose", "ghee", "buttermilk", "kefir", "parmesan"
        },
            ["Nuts"] = new[]
            {
            "almond", "almonds", "hazelnut", "hazelnuts", "walnut", "walnuts",
            "cashew", "cashews", "pecan", "pecans", "pistachio", "pistachios",
            "macadamia", "brazil nut", "brazil nuts", "nuts"
        },
            ["Celery"] = new[]
            {
            "celery", "celeriac"
        },
            ["Mustard"] = new[]
            {
            "mustard", "dijon"
        },
            ["Sesame"] = new[]
            {
            "sesame", "tahini"
        },
            ["Sulphur dioxide and sulphites"] = new[]
            {
            "sulphite", "sulphites", "sulfite", "sulfites",
            "sulfur dioxide", "e220", "e221", "e222", "e223", "e224", "e225", "e226", "e227", "e228",
            "metabisulfite", "metabisulphite"
        },
            ["Lupin"] = new[]
            {
            "lupin"
        },
            ["Molluscs"] = new[]
            {
            "mollusc", "molluscs", "mussel", "mussels", "clam", "clams",
            "oyster", "oysters", "squid", "octopus", "scallop", "scallops"
        }
        };


        private static readonly string[] MeatFishSeafoodKeywords =
        {
        "meat", "beef", "pork", "chicken", "turkey", "duck", "lamb", "veal",
        "bacon", "ham", "sausage", "salami", "pepperoni", "prosciutto", "chorizo",
        "gelatin", "gelatine", "lard", "tallow",

        "fish", "salmon", "tuna", "anchovy", "anchovies", "cod", "sardine", "sardines",
        "shrimp", "prawn", "crab", "lobster", "mussel", "mussels", "clam", "clams", "oyster", "oysters",
        "squid", "octopus", "scallop", "scallops"
    };

        private static readonly string[] EggKeywords = { "egg", "eggs", "albumen", "mayonnaise", "mayo" };

        private static readonly string[] DairyKeywords =
        {
        "milk", "butter", "cheese", "cream", "yogurt", "yoghurt", "whey", "casein", "lactose", "ghee", "parmesan"
    };

        private static readonly string[] HoneyKeywords = { "honey" };

        public Task<FoodAnalysisResult> AnalyzeAsync(string text, CancellationToken ct = default)
        {
            text ??= "";
            var normalized = Normalize(text);

            var tokens = Tokenize(normalized);

            var allergenFindings = new List<AllergenFinding>();

            foreach (var (allergen, keywords) in AllergenKeywords)
            {
                var triggers = new List<string>();

                foreach (var kw in keywords)
                {
                    if (IsMatch(normalized, tokens, kw))
                        triggers.Add(kw);
                }

                if (triggers.Count > 0)
                    allergenFindings.Add(new AllergenFinding(allergen, triggers.Distinct().ToList()));
            }

            var hasGluten = allergenFindings.Any(a => a.Allergen == "Cereals containing gluten");
            var isGlutenFree = !hasGluten;

            var hasMeatFishSeafood = MeatFishSeafoodKeywords.Any(kw => IsMatch(normalized, tokens, kw));
            var hasEgg = EggKeywords.Any(kw => IsMatch(normalized, tokens, kw));
            var hasDairy = DairyKeywords.Any(kw => IsMatch(normalized, tokens, kw));
            var hasHoney = HoneyKeywords.Any(kw => IsMatch(normalized, tokens, kw));

            var isVegetarian = !hasMeatFishSeafood;
            var isVegan = !hasMeatFishSeafood && !hasEgg && !hasDairy && !hasHoney;

            return Task.FromResult(new FoodAnalysisResult(
                Allergens: allergenFindings.OrderBy(a => a.Allergen).ToList(),
                IsVegan: isVegan,
                IsVegetarian: isVegetarian,
                IsGlutenFree: isGlutenFree
            ));
        }

        private static string Normalize(string input)
        {

            input = input.ToLowerInvariant();

            Span<char> buffer = stackalloc char[input.Length];
            var j = 0;

            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                buffer[j++] = char.IsLetterOrDigit(c) ? c : ' ';
            }


            var cleaned = new string(buffer[..j]);
            while (cleaned.Contains("  "))
                cleaned = cleaned.Replace("  ", " ");

            return cleaned.Trim();
        }

        private static HashSet<string> Tokenize(string normalized)
        {
            var raw = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var set = new HashSet<string>(StringComparer.Ordinal);

            foreach (var t in raw)
            {
                set.Add(t);

                if (t.EndsWith('s') && t.Length > 3)
                    set.Add(t.TrimEnd('s'));
                if (t.EndsWith("es") && t.Length > 4)
                    set.Add(t[..^2]);
            }

            return set;
        }

        private static bool IsMatch(string normalized, HashSet<string> tokens, string keyword)
        {
            keyword = keyword.ToLowerInvariant().Trim();

            if (keyword.Contains(' '))
                return normalized.Contains(keyword, StringComparison.Ordinal);

            return tokens.Contains(keyword);
        }
    }
}
