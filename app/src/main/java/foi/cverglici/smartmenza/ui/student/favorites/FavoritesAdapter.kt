package foi.cverglici.smartmenza.ui.student.favorites

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.TextView
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.core.data.model.favorite.FavoriteDish
import foi.cverglici.smartmenza.R

class FavoritesAdapter(
    private val onDishClicked: (FavoriteDish) -> Unit,
    private val onRemoveFavorite: (FavoriteDish) -> Unit
) : ListAdapter<FavoriteDish, FavoritesAdapter.FavoriteViewHolder>(FavoriteDiffCallback()) {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): FavoriteViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_favorite_dish, parent, false)
        return FavoriteViewHolder(view)
    }

    override fun onBindViewHolder(holder: FavoriteViewHolder, position: Int) {
        holder.bind(getItem(position))
    }

    inner class FavoriteViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val titleText: TextView = itemView.findViewById(R.id.dishTitle)
        private val descriptionText: TextView = itemView.findViewById(R.id.dishDescription)
        private val priceText: TextView = itemView.findViewById(R.id.dishPrice)
        private val caloriesText: TextView = itemView.findViewById(R.id.caloriesValue)
        private val removeButton: ImageButton = itemView.findViewById(R.id.removeFavoriteButton)

        fun bind(dish: FavoriteDish) {
            titleText.text = dish.title
            descriptionText.text = dish.description ?: "Nema opisa"
            priceText.text = String.format("%.2f €", dish.price)
            caloriesText.text = "${dish.calories ?: 0} kcal"

            itemView.setOnClickListener {
                onDishClicked(dish)
            }

            removeButton.setOnClickListener {
                onRemoveFavorite(dish)
            }
        }
    }

    private class FavoriteDiffCallback : DiffUtil.ItemCallback<FavoriteDish>() {
        override fun areItemsTheSame(oldItem: FavoriteDish, newItem: FavoriteDish): Boolean {
            return oldItem.dishId == newItem.dishId
        }

        override fun areContentsTheSame(oldItem: FavoriteDish, newItem: FavoriteDish): Boolean {
            return oldItem == newItem
        }
    }
}