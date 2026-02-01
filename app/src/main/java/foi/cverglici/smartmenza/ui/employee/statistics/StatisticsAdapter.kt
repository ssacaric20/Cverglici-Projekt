package foi.cverglici.smartmenza.ui.employee.statistics

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.core.data.model.employee.statistics.DishStatisticsResponse
import foi.cverglici.smartmenza.R

class StatisticsAdapter(
    private val onDishClicked: (DishStatisticsResponse) -> Unit
) : ListAdapter<DishStatisticsResponse, StatisticsAdapter.StatisticsViewHolder>(DiffCallback()) {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): StatisticsViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_statistics_dish, parent, false)
        return StatisticsViewHolder(view, onDishClicked)
    }

    override fun onBindViewHolder(holder: StatisticsViewHolder, position: Int) {
        holder.bind(getItem(position), position + 1)
    }

    class StatisticsViewHolder(
        itemView: View,
        private val onDishClicked: (DishStatisticsResponse) -> Unit
    ) : RecyclerView.ViewHolder(itemView) {

        private val rankText: TextView = itemView.findViewById(R.id.rankText)
        private val dishTitle: TextView = itemView.findViewById(R.id.dishTitle)
        private val averageRating: TextView = itemView.findViewById(R.id.averageRating)
        private val ratingsCount: TextView = itemView.findViewById(R.id.ratingsCount)
        private val favoriteCount: TextView = itemView.findViewById(R.id.favoriteCount)

        fun bind(dish: DishStatisticsResponse, rank: Int) {
            rankText.text = rank.toString()
            dishTitle.text = dish.title

            if (dish.averageRating != null) {
                averageRating.text = String.format("%.1f", dish.averageRating)
            } else {
                averageRating.text = "-"
            }

            ratingsCount.text = "(${dish.ratingsCount})"
            favoriteCount.text = dish.favoriteCount.toString()

            itemView.setOnClickListener {
                onDishClicked(dish)
            }
        }
    }

    private class DiffCallback : DiffUtil.ItemCallback<DishStatisticsResponse>() {
        override fun areItemsTheSame(
            oldItem: DishStatisticsResponse,
            newItem: DishStatisticsResponse
        ): Boolean {
            return oldItem.dishId == newItem.dishId
        }

        override fun areContentsTheSame(
            oldItem: DishStatisticsResponse,
            newItem: DishStatisticsResponse
        ): Boolean {
            return oldItem == newItem
        }
    }
}