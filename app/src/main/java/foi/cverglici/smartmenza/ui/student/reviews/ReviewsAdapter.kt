package foi.cverglici.smartmenza.ui.student.reviews

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.PopupMenu
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import foi.cverglici.smartmenza.R
import foi.cverglici.core.data.model.student.reviews.ReviewResponse

class ReviewsAdapter(
    private val currentUserId: Int,
    private val onEdit: (ReviewResponse) -> Unit,
    private val onDelete: (ReviewResponse) -> Unit
) : RecyclerView.Adapter<ReviewsAdapter.VH>() {

    private val items = mutableListOf<ReviewResponse>()

    fun submit(list: List<ReviewResponse>) {
        items.clear()
        items.addAll(list)
        notifyDataSetChanged()
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): VH {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_review, parent, false)
        return VH(view)
    }

    override fun onBindViewHolder(holder: VH, position: Int) {
        holder.bind(items[position], currentUserId, onEdit, onDelete)
    }

    override fun getItemCount(): Int = items.size

    class VH(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val tvName: TextView = itemView.findViewById(R.id.tvReviewerName)
        private val tvRating: TextView = itemView.findViewById(R.id.tvRatingValue)
        private val tvText: TextView = itemView.findViewById(R.id.tvReviewText)
        private val tvDate: TextView = itemView.findViewById(R.id.tvReviewDate)
        private val btnMore: ImageView = itemView.findViewById(R.id.btnMore)

        fun bind(
            item: ReviewResponse,
            currentUserId: Int,
            onEdit: (ReviewResponse) -> Unit,
            onDelete: (ReviewResponse) -> Unit
        ) {
            tvName.text = item.userName
            tvRating.text = item.rating.toString()
            tvText.text = item.comment
            tvDate.text = item.createdAt.take(10) // YYYY-MM-DD iz ISO stringa

            val isMine = item.userId == currentUserId
            btnMore.visibility = if (isMine) View.VISIBLE else View.GONE


            btnMore.setOnClickListener { v ->
                val popup = PopupMenu(v.context, v)
                popup.menu.add(0, 1, 0, v.context.getString(R.string.edit))
                popup.menu.add(0, 2, 1, v.context.getString(R.string.delete))
                popup.setOnMenuItemClickListener { menuItem ->
                    when (menuItem.itemId) {
                        1 -> {
                            onEdit(item)
                            true
                        }
                        2 -> {
                            onDelete(item)
                            true
                        }
                        else -> false
                    }
                }
                popup.show()
            }
        }
    }
}
