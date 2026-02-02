package foi.cverglici.smartmenza.ui.student.goals

import android.app.AlertDialog
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.core.data.api.student.dailymenu.RetrofitDish
import foi.cverglici.core.data.model.student.dailymenu.DailyMenuItem
import foi.cverglici.core.data.model.student.dailyfoodintake.DailyFoodIntakeResponse
import foi.cverglici.core.data.model.student.nutritiongoal.NutritionGoalResponse
import foi.cverglici.core.data.model.student.nutritiongoal.SetNutritionGoalRequest
import foi.cverglici.core.data.model.student.nutritiongoal.TodayNutritionProgressResponse
import foi.cverglici.smartmenza.R
import foi.cverglici.smartmenza.session.SessionTokenProvider
import kotlinx.coroutines.launch
import androidx.core.widget.doAfterTextChanged
import android.widget.ArrayAdapter


class GoalsFragment : Fragment() {

    private lateinit var goalsManager: GoalsManager

    private lateinit var dailyFoodIntakeManager: DailyFoodIntakeManager

    private lateinit var btnEditGoals: Button
    private lateinit var btnAddDish: Button

    private lateinit var progressCalories: ProgressBar
    private lateinit var progressProteins: ProgressBar
    private lateinit var progressCarbs: ProgressBar
    private lateinit var progressFats: ProgressBar

    private lateinit var txtCaloriesValue: TextView
    private lateinit var txtProteinsValue: TextView
    private lateinit var txtCarbsValue: TextView
    private lateinit var txtFatsValue: TextView

    private lateinit var rvTodayFoods: RecyclerView
    private lateinit var txtTodayCount: TextView
    private lateinit var txtEmptyToday: TextView
    private lateinit var txtOverGoalBanner: TextView
    private lateinit var todayAdapter: TodayFoodIntakeAdapter

    private lateinit var cardCalories: View
    private lateinit var cardProtein: View
    private lateinit var cardCarbs: View
    private lateinit var cardFat: View

    private lateinit var badgeCalories: TextView
    private lateinit var badgeProtein: TextView
    private lateinit var badgeCarbs: TextView
    private lateinit var badgeFat: TextView

    private lateinit var valueCalories: TextView
    private lateinit var valueProtein: TextView
    private lateinit var valueCarbs: TextView
    private lateinit var valueFat: TextView

    private lateinit var progCalories: ProgressBar
    private lateinit var progProtein: ProgressBar
    private lateinit var progCarbs: ProgressBar
    private lateinit var progFat: ProgressBar

    private var lastGoal: NutritionGoalResponse? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        return inflater.inflate(R.layout.goals_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        goalsManager = GoalsManager(requireContext(), viewLifecycleOwner)
        dailyFoodIntakeManager = DailyFoodIntakeManager(requireContext(), viewLifecycleOwner)

        bindViews(view)
        setupTodayList()

        btnEditGoals.setOnClickListener { showEditGoalsDialog() }
        btnAddDish.setOnClickListener { showAddDishDialogFromTodayMenu() }

        refreshAll()
    }

    private fun bindViews(view: View) {
        btnEditGoals = view.findViewById(R.id.btnEditGoals)
        btnAddDish = view.findViewById(R.id.btnAddDish)



        rvTodayFoods = view.findViewById(R.id.rvTodayFoods)
        txtTodayCount = view.findViewById(R.id.txtTodayCount)
        txtEmptyToday = view.findViewById(R.id.txtEmptyToday)
        txtOverGoalBanner = view.findViewById(R.id.txtOverGoalBanner)

        cardCalories = view.findViewById(R.id.cardCalories)
        cardProtein = view.findViewById(R.id.cardProtein)
        cardCarbs = view.findViewById(R.id.cardCarbs)
        cardFat = view.findViewById(R.id.cardFat)

        fun bindCard(card: View): Triple<TextView, TextView, ProgressBar> {
            val badge = card.findViewById<TextView>(R.id.txtMacroBadge)
            val value = card.findViewById<TextView>(R.id.txtMacroValue)
            val prog = card.findViewById<ProgressBar>(R.id.progressMacro)
            return Triple(badge, value, prog)
        }

        val (bCal, vCal, pCal) = bindCard(cardCalories)
        badgeCalories = bCal; valueCalories = vCal; progCalories = pCal

        val (bP, vP, pP) = bindCard(cardProtein)
        badgeProtein = bP; valueProtein = vP; progProtein = pP

        val (bC, vC, pC) = bindCard(cardCarbs)
        badgeCarbs = bC; valueCarbs = vC; progCarbs = pC

        val (bF, vF, pF) = bindCard(cardFat)
        badgeFat = bF; valueFat = vF; progFat = pF
    }

    private fun setupTodayList() {
        todayAdapter = TodayFoodIntakeAdapter(emptyList()) { item ->

            todayAdapter.removeById(item.dailyFoodIntakeId)

            txtTodayCount.text = "Današnji obroci: ${todayAdapter.itemCount}"
            txtEmptyToday.visibility = if (todayAdapter.itemCount == 0) View.VISIBLE else View.GONE

            dailyFoodIntakeManager.deleteItem(
                id = item.dailyFoodIntakeId,
                onSuccess = {
                    toast("Obrisano.")
                    refreshAll()
                },
                onError = { err ->
                    toast(err)

                    refreshAll()
                }
            )
        }
        rvTodayFoods.layoutManager = LinearLayoutManager(requireContext())
        rvTodayFoods.adapter = todayAdapter
    }

    private fun refreshAll() {
        loadGoalAndProgress()
        loadTodayFoodIntakes()
    }

    private fun loadGoalAndProgress() {
        goalsManager.loadGoalAndTodayProgress(
            onSuccess = { goal, today ->
                lastGoal = goal
                renderProgress(goal, today)
            },
            onError = { msg -> toast(msg) }
        )
    }

    private fun renderProgress(goal: NutritionGoalResponse, today: TodayNutritionProgressResponse) {

        // Labels
        badgeCalories.text = "KALORIJE"
        badgeProtein.text = "PROTEIN"
        badgeCarbs.text = "UGLJIKOHIDRATI"
        badgeFat.text = "MASTI"

        // Calories: sivi/neutralno
        badgeCalories.setTextColor(resources.getColor(R.color.text_primary, null))
        badgeCalories.setBackgroundColor(resources.getColor(R.color.white, null))

        // Protein
        badgeProtein.setTextColor(resources.getColor(R.color.protein_text, null))
        badgeProtein.setBackgroundColor(resources.getColor(R.color.protein_bg, null))

        // Carbs
        badgeCarbs.setTextColor(resources.getColor(R.color.carbs_text, null))
        badgeCarbs.setBackgroundColor(resources.getColor(R.color.carbs_bg, null))

        // Fat
        badgeFat.setTextColor(resources.getColor(R.color.fat_text, null))
        badgeFat.setBackgroundColor(resources.getColor(R.color.fat_bg, null))

        // Values
        valueCalories.text = "${today.caloriesConsumed} / ${goal.caloriesGoal} kcal"
        valueProtein.text = "${fmt1(today.proteinsConsumed)} / ${fmt1(goal.proteinsGoal)} g"
        valueCarbs.text = "${fmt1(today.carbohydratesConsumed)} / ${fmt1(goal.carbohydratesGoal)} g"
        valueFat.text = "${fmt1(today.fatsConsumed)} / ${fmt1(goal.fatsGoal)} g"

        // Progress (0–1000)
        progCalories.progress = ratioTo1000(today.caloriesConsumed.toDouble(), goal.caloriesGoal.toDouble())
        progProtein.progress = ratioTo1000(today.proteinsConsumed, goal.proteinsGoal)
        progCarbs.progress = ratioTo1000(today.carbohydratesConsumed, goal.carbohydratesGoal)
        progFat.progress = ratioTo1000(today.fatsConsumed, goal.fatsGoal)

        // Over-goal = crveno + banner
        val overCalories = today.caloriesConsumed > goal.caloriesGoal
        val overProtein = today.proteinsConsumed > goal.proteinsGoal
        val overCarbs = today.carbohydratesConsumed > goal.carbohydratesGoal
        val overFats = today.fatsConsumed > goal.fatsGoal

        progCalories.progressDrawable = resources.getDrawable(
            if (overCalories) R.drawable.progress_red else R.drawable.progress_gradient_orange_purple, null
        )
        progProtein.progressDrawable = resources.getDrawable(
            if (overProtein) R.drawable.progress_red else R.drawable.progress_gradient_orange_purple, null
        )
        progCarbs.progressDrawable = resources.getDrawable(
            if (overCarbs) R.drawable.progress_red else R.drawable.progress_gradient_orange_purple, null
        )
        progFat.progressDrawable = resources.getDrawable(
            if (overFats) R.drawable.progress_red else R.drawable.progress_gradient_orange_purple, null
        )

        txtOverGoalBanner.visibility = if (overCalories || overProtein || overCarbs || overFats) View.VISIBLE else View.GONE
    }


    private fun showEditGoalsDialog() {
        val dialogView = LayoutInflater.from(requireContext()).inflate(R.layout.dialog_edit_goals, null)

        val etCalories = dialogView.findViewById<EditText>(R.id.etCalories)
        val etProteins = dialogView.findViewById<EditText>(R.id.etProteins)
        val etCarbs = dialogView.findViewById<EditText>(R.id.etCarbs)
        val etFats = dialogView.findViewById<EditText>(R.id.etFats)
        val txtRecommended = dialogView.findViewById<TextView>(R.id.txtRecommendedMacros)

        val spinner = dialogView.findViewById<Spinner>(R.id.spinnerMacroProfile)

        val profiles = MacroProfile.values().toList()
        spinner.adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_dropdown_item,
            profiles
        )

        var selectedProfile = MacroProfile.BALANCED
        spinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {
                selectedProfile = profiles[position]

                // refresh preporuke ako kalorije već postoje
                val cal = etCalories.text.toString().trim().toIntOrNull()
                if (cal != null && cal > 0) {
                    txtRecommended.text = buildRecommendedText(cal, selectedProfile)
                }
            }

            override fun onNothingSelected(parent: AdapterView<*>?) {}
        }



        lastGoal?.let {
            etCalories.setText(it.caloriesGoal.toString())
            etProteins.setText(it.proteinsGoal.toString())
            etCarbs.setText(it.carbohydratesGoal.toString())
            etFats.setText(it.fatsGoal.toString())
        }

        etCalories.doAfterTextChanged { s ->
            val cal = s?.toString()?.trim()?.toIntOrNull()
            if (cal == null || cal <= 0) {
                txtRecommended.text = "Preporuka će se prikazati nakon unosa kalorija."
                return@doAfterTextChanged
            }
            txtRecommended.text = buildRecommendedText(cal, selectedProfile)
        }



        AlertDialog.Builder(requireContext())
            .setTitle("Uredi ciljeve")
            .setView(dialogView)
            .setNegativeButton("Odustani", null)
            .setPositiveButton("Spremi") { _, _ ->
                val cal = etCalories.text.toString().trim().toIntOrNull()
                val p = etProteins.text.toString().trim().toDoubleOrNull()
                val c = etCarbs.text.toString().trim().toDoubleOrNull()
                val f = etFats.text.toString().trim().toDoubleOrNull()

                if (cal == null || p == null || c == null || f == null) {
                    toast("Unesi brojčane vrijednosti.")
                    return@setPositiveButton
                }


                val msg = validateGoalInputs(cal, p, f, c)
                if (msg != null) {
                    toast(msg)
                    return@setPositiveButton
                }

                // 4 kcal / g protein, 4 kcal / g carbs, 9 kcal / g fat
                val minCaloriesFromMacros = (4 * p) + (4 * c) + (9 * f)

                if (cal.toDouble() < minCaloriesFromMacros) {
                    toast(
                        "Kalorije su preniske za zadane makroe.\n" +
                                "Minimum je ${minCaloriesFromMacros.toInt()} kcal (4P + 4C + 9F)."
                    )
                    return@setPositiveButton
                }

                //ako user zada kalorije, makroi ne smiju "davati" više/manje kcal od cilja (uz toleranciju)
                val caloriesFromMacros = (4 * p) + (4 * c) + (9 * f)
                val toleranceKcal = 25.0  // dopušteno odstupanje zbog decimala/zaokruživanja

                if (kotlin.math.abs(cal.toDouble() - caloriesFromMacros) > toleranceKcal) {
                    toast(
                        "Kalorije i makroi nisu usklađeni.\n" +
                                "Makroi daju ~${caloriesFromMacros.toInt()} kcal (4P + 4C + 9F).\n" +
                                "Prilagodi makroe ili kalorije (tolerancija ±${toleranceKcal.toInt()} kcal)."
                    )
                    return@setPositiveButton
                }

                goalsManager.saveGoal(
                    request = SetNutritionGoalRequest(
                        caloriesGoal = cal,
                        proteinsGoal = p,
                        fatsGoal = f,
                        carbohydratesGoal = c
                    ),
                    onSuccess = {
                        toast("Ciljevi spremljeni.")
                        refreshAll()
                    },
                    onError = { err -> toast(err) }
                )
            }
            .show()
    }

    private fun loadTodayFoodIntakes() {
        dailyFoodIntakeManager.loadToday(
            onSuccess = { items -> renderTodayIntakes(items) },
            onError = { msg -> toast(msg) }
        )
    }

    private fun renderTodayIntakes(items: List<DailyFoodIntakeResponse>) {
        txtTodayCount.text = "Današnji obroci: ${items.size}"
        txtEmptyToday.visibility = if (items.isEmpty()) View.VISIBLE else View.GONE
        todayAdapter.update(items)
    }

    private fun showAddDishDialogFromTodayMenu() {
        val dialogView = LayoutInflater.from(requireContext()).inflate(R.layout.dialog_select_dish, null)
        val recycler = dialogView.findViewById<RecyclerView>(R.id.recyclerDishes)
        val txtEmpty = dialogView.findViewById<TextView>(R.id.txtEmptyDishes)

        recycler.layoutManager = LinearLayoutManager(requireContext())

        val dialog = AlertDialog.Builder(requireContext())
            .setView(dialogView)
            .setNegativeButton("Odustani", null)
            .create()

        val tokenProvider = SessionTokenProvider(requireContext())
        val dishService = RetrofitDish.create(tokenProvider)

        lifecycleScope.launch {
            try {
                val resp = dishService.getTodayMenu(category = "")
                if (!resp.isSuccessful) {
                    txtEmpty.visibility = View.VISIBLE
                    txtEmpty.text = "Greška pri dohvaćanju menija."
                    return@launch
                }

                val menuItems: List<DailyMenuItem> = resp.body().orEmpty()
                if (menuItems.isEmpty()) {
                    txtEmpty.visibility = View.VISIBLE
                    txtEmpty.text = "Nema jela u današnjem meniju."
                    return@launch
                }

                txtEmpty.visibility = View.GONE

                recycler.adapter = SelectDishFromMenuAdapter(menuItems) { item ->
                    dailyFoodIntakeManager.addDish(
                        dishId = item.dishId,
                        onSuccess = {
                            toast("Dodano.")
                            dialog.dismiss()
                            refreshAll()
                        },
                        onError = { msg -> toast(msg) }
                    )
                }
            } catch (e: Exception) {
                txtEmpty.visibility = View.VISIBLE
                txtEmpty.text = "Greška: ${e.message}"
            }
        }
        dialog.show()
    }

    private fun validateGoalInputs(cal: Int, p: Double, f: Double, c: Double): String? {
        if (cal < 0 || cal > 10000) return "Kalorije moraju biti 0–10000."
        if (p < 0.0 || p > 1000.0) return "Proteini moraju biti 0–1000."
        if (f < 0.0 || f > 1000.0) return "Masti moraju biti 0–1000."
        if (c < 0.0 || c > 1000.0) return "Ugljikohidrati moraju biti 0–1000."
        return null
    }

    private fun ratioTo1000(consumed: Double, goal: Double): Int {
        if (goal <= 0.0) return 0
        val r = (consumed / goal).coerceIn(0.0, 1.0)
        return (r * 1000).toInt()
    }

    private fun fmt1(v: Double): String = String.format("%.1f", v)

    private fun toast(msg: String) {
        Toast.makeText(requireContext(), msg, Toast.LENGTH_LONG).show()
    }

    private fun buildRecommendedText(calories: Int, profile: MacroProfile): String {
        fun gramsFromPct(pct: Int, kcalPerGram: Int): Double =
            (calories * (pct / 100.0)) / kcalPerGram

        val carbsG = gramsFromPct(profile.carbsPct, 4)
        val proteinG = gramsFromPct(profile.proteinPct, 4)
        val fatG = gramsFromPct(profile.fatPct, 9)

        fun r(v: Double) = String.format("%.0f", v)

        // AMDR ranges (informativno)
        val carbsMin = calories * 0.45 / 4.0
        val carbsMax = calories * 0.65 / 4.0
        val proteinMin = calories * 0.10 / 4.0
        val proteinMax = calories * 0.35 / 4.0
        val fatMin = calories * 0.20 / 9.0
        val fatMax = calories * 0.35 / 9.0

        return """
        Profil: ${profile.label} (${profile.carbsPct}/${profile.proteinPct}/${profile.fatPct})
        Preporuka:
        Carbs ${r(carbsG)} g | Protein ${r(proteinG)} g | Fat ${r(fatG)} g

        AMDR rasponi (općenito):
        Carbs ${r(carbsMin)}–${r(carbsMax)} g
        Protein ${r(proteinMin)}–${r(proteinMax)} g
        Fat ${r(fatMin)}–${r(fatMax)} g
    """.trimIndent()
    }


    private enum class MacroProfile(val label: String, val carbsPct: Int, val proteinPct: Int, val fatPct: Int) {
        BALANCED("Balansirano (zdravlje)", 50, 25, 25),
        WEIGHT_LOSS("Mršavljenje", 35, 35, 30),
        MUSCLE_GAIN("Gradnja mišića", 40, 35, 25),
        CARDIO("Kardio / izdržljivost", 55, 25, 20);

        override fun toString(): String = label
    }


}
