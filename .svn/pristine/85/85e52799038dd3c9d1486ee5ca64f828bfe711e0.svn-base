package avatar.module.item;

import avatar.entity.item.ItemEntity;
import avatar.util.GameData;

import java.util.Arrays;
import java.util.List;

/**
 * 资源道具的数据操作类
 */
public class ItemDao {
    private static final ItemDao instance = new ItemDao();

    static final ItemDao getInstance(){
        return instance;
    }

    private final String key = "item_%d";

    private String getKey(int userId){
        return String.format(key , userId);
    }

    /**
     * 获取当前玩家所有的道具
     * @param userId        用户id
     */
    public List<ItemEntity> loadAllItems(int userId){
        List<ItemEntity> items = loadCache(userId);
        if(items != null && items.size() > 0){
            return items;
        }
        items = loadAllDB(userId);
        if(items != null && items.size() > 0){
            setCache(userId , items);
        }
        return items;
    }

    /**
     * 添加单个道具
     */
    public boolean addItem(int userId , ItemEntity itemEntity){
        removeCache(userId);
        List<ItemEntity> add = Arrays.asList(itemEntity);
        return addItemDB(add);
    }

    /**
     * 更新单个道具
     */
    public boolean updateItem(int userId , ItemEntity itemEntity){
        removeCache(userId);
        return updateItemDB(itemEntity);
    }

    /**
     * 更新多个道具
     * @param userId
     * @param itemEntities
     * @return
     */
    public boolean updateBatchItems(int userId , List<ItemEntity> itemEntities){
        removeCache(userId);
        return updateBatchItemDB(itemEntities);
    }

    public boolean insertBatchItems(int userId , List<ItemEntity> addList){
        removeCache(userId);
        return addItemDB(addList);
    }


    private List<ItemEntity> loadCache(int userId){
        return GameData.getCache().getList(getKey(userId));
    }

    private void setCache(int userId , List<ItemEntity> list){
        GameData.getCache().setList(getKey(userId) , list);
    }

    private void removeCache(int userId){
        GameData.getCache().removeCache(getKey(userId));
    }

    private List<ItemEntity> loadAllDB(int userId){
        String sql = "select * from Item where userId = ?;";
        return GameData.getDB().list(ItemEntity.class , sql , new Object[]{userId});
    }

    private boolean addItemDB(List<ItemEntity> itemEntity){
        boolean ret = GameData.getDB().insert(itemEntity);
        if(!ret){
            //log
        }
        return ret;
    }

    private boolean updateItemDB(ItemEntity itemEntity){
        boolean ret = GameData.getDB().update(itemEntity);
        if(!ret){
            //log
        }
        return ret;
    }

    private boolean updateBatchItemDB(List<ItemEntity> list){
        boolean ret = GameData.getDB().update(list);
        if(!ret){
            //log
        }
        return ret;
    }
}
