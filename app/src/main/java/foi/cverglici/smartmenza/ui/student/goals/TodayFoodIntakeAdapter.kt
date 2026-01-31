package foi.cverglici.smartmenza.ui.student.goals

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.core.data.model.student.dailyfoodintake.DailyFoodIntakeResponse
import foi.cverglici.smartmenza.R
import android.widget.ImageView
import coil.load


class TodayFoodIntakeAdapter(
    private var items: List<DailyFoodIntakeResponse>,
    private val onDeleteClick: (DailyFoodIntakeResponse) -> Unit
) : RecyclerView.Adapter<TodayFoodIntakeAdapter.VH>() {

    inner class VH(v: View) : RecyclerView.ViewHolder(v) {
        val title: TextView = v.findViewById(R.id.txtTodayDishTitle)
        val deleteBtn: ImageButton = v.findViewById(R.id.btnDeleteTodayDish)

        val img: ImageView = v.findViewById(R.id.imgTodayDish)
        val badgeKcal: TextView = v.findViewById(R.id.badgeTodayCalories)
        val badgeP: TextView = v.findViewById(R.id.badgeTodayProtein)
        val badgeC: TextView = v.findViewById(R.id.badgeTodayCarbs)
        val badgeF: TextView = v.findViewById(R.id.badgeTodayFat)
    }


    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): VH {
        val v = LayoutInflater.from(parent.context).inflate(R.layout.item_today_food, parent, false)
        return VH(v)
    }

    override fun onBindViewHolder(holder: VH, position: Int) {
        val item = items[position]

        holder.title.text = item.dishTitle
        holder.badgeKcal.text = "${item.calories} kcal"
        holder.badgeP.text = "${fmt1(item.protein)}g P"
        holder.badgeC.text = "${fmt1(item.carbohydrates)}g C"
        holder.badgeF.text = "${fmt1(item.fat)}g F"


        holder.img.load(item.imgPath) {
            crossfade(true)
            placeholder(android.R.drawable.ic_menu_gallery)
            error(android.R.drawable.ic_menu_gallery)
        }

        holder.deleteBtn.setOnClickListener { onDeleteClick(item) }
    }


    override fun getItemCount(): Int = items.size

    fun update(newItems: List<DailyFoodIntakeResponse>) {
        items = newItems
        notifyDataSetChanged()
    }

    private fun fmt1(v: Double): String = String.format("%.1f", v)


    fun removeById(id: Int) {
        val idx = items.indexOfFirst { it.dailyFoodIntakeId == id }
        if (idx >= 0) {
            items = items.toMutableList().also { it.removeAt(idx) }
            notifyItemRemoved(idx)
        }
    }

}
