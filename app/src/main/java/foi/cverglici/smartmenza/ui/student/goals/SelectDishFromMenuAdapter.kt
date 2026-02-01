package foi.cverglici.smartmenza.ui.student.goals

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import coil.load
import foi.cverglici.core.data.model.student.dailymenu.DailyMenuItem
import foi.cverglici.smartmenza.R

class SelectDishFromMenuAdapter(
    private val items: List<DailyMenuItem>,
    private val onClick: (DailyMenuItem) -> Unit
) : RecyclerView.Adapter<SelectDishFromMenuAdapter.VH>() {

    inner class VH(v: View) : RecyclerView.ViewHolder(v) {
        val img: ImageView = v.findViewById(R.id.imgDish)
        val title: TextView = v.findViewById(R.id.txtDishTitle)
        val desc: TextView = v.findViewById(R.id.txtDishDesc)

        val badgeCalories: TextView = v.findViewById(R.id.badgeCalories)
        val badgeProtein: TextView = v.findViewById(R.id.badgeProtein)
        val badgeCarbs: TextView = v.findViewById(R.id.badgeCarbs)
        val badgeFat: TextView = v.findViewById(R.id.badgeFat)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): VH {
        val v = LayoutInflater.from(parent.context).inflate(R.layout.item_select_dish, parent, false)
        return VH(v)
    }

    override fun onBindViewHolder(holder: VH, position: Int) {
        val item = items[position]
        val d = item.dish

        holder.title.text = d.title
        holder.desc.text = d.description

        holder.badgeCalories.text = "${d.calories} kcal"
        holder.badgeProtein.text = "${fmt0(d.protein)}g P"
        holder.badgeCarbs.text = "${fmt0(d.carbohydrates)}g C"
        holder.badgeFat.text = "${fmt0(d.fat)}g F"

        holder.img.load(d.imgPath) {
            crossfade(true)
            placeholder(android.R.drawable.ic_menu_gallery)
            error(android.R.drawable.ic_menu_gallery)
        }

        holder.itemView.setOnClickListener { onClick(item) }
    }

    override fun getItemCount(): Int = items.size

    private fun fmt0(v: Double): String = String.format("%.0f", v)
}
