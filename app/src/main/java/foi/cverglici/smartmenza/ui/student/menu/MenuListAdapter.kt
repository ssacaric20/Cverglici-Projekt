package foi.cverglici.smartmenza.ui.student.menu

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.core.menu.model.DailyMenuItem
import foi.cverglici.smartmenza.R
import java.util.Locale

class MenuListAdapter(
    private val onItemClick: (DailyMenuItem) -> Unit
) : ListAdapter<DailyMenuItem, MenuListAdapter.MenuViewHolder>(DiffCallback()) {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MenuViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_menu_dish, parent, false)
        return MenuViewHolder(view, onItemClick)
    }

    override fun onBindViewHolder(holder: MenuViewHolder, position: Int) {
        holder.bind(getItem(position))
    }

    class MenuViewHolder(
        itemView: View,
        private val onItemClick: (DailyMenuItem) -> Unit
    ) : RecyclerView.ViewHolder(itemView) {

        private val titleText: TextView = itemView.findViewById(R.id.dishTitle)
        private val descriptionText: TextView = itemView.findViewById(R.id.dishDescription)
        private val priceText: TextView = itemView.findViewById(R.id.dishPrice)
        private val caloriesText: TextView = itemView.findViewById(R.id.caloriesValue)

        fun bind(menuItem: DailyMenuItem) {
            val dish = menuItem.dish

            titleText.text = dish.title
            descriptionText.text = dish.description ?: "Nema opisa"
            priceText.text = String.format(Locale.getDefault(), "%.2f €", dish.price)
            caloriesText.text = "${dish.calories} kcal"

            itemView.setOnClickListener {
                onItemClick(menuItem)
            }
        }
    }

    private class DiffCallback : DiffUtil.ItemCallback<DailyMenuItem>() {
        override fun areItemsTheSame(oldItem: DailyMenuItem, newItem: DailyMenuItem): Boolean {
            return oldItem.dailyMenuId == newItem.dailyMenuId
        }

        override fun areContentsTheSame(oldItem: DailyMenuItem, newItem: DailyMenuItem): Boolean {
            return oldItem == newItem
        }
    }
}