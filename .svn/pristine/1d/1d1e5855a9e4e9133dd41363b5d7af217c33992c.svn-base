package avatar.module.item;

import avatar.entity.item.ItemEntity;
import avatar.protobuf.Cmd;
import avatar.net.session.Session;
import avatar.protobuf.Hall;
import avatar.util.GameData;

import java.util.*;

/**
 * 玩家道具业务操作接口
 */
public class ItemService {

    private static final ItemService instance = new ItemService();
    public static final ItemService getInstance(){
        return instance;
    }

    /**
     * 获取玩家所有的资源道具
     * @return
     */
    public List<ItemEntity> getAllUserItems(int userId){
        List<ItemEntity> list = ItemDao.getInstance().loadAllItems(userId);
        if(list == null){
            return new ArrayList<>();
        }
        return list;
    }

    /**
     * 根据道具类型获取到玩家道具信息
     * @param userId        玩家id
     * @param itemType      道具类型
     */
    public ItemEntity getItemByType(int userId , int itemType){
        List<ItemEntity> list = getAllUserItems(userId);
        if(list.size() > 0){
            for(ItemEntity itemEntity : list){
                if(itemEntity.getItemType() == itemType){
                    return itemEntity;
                }
            }
            return null;
        }else{
            return null;
        }
    }

    /**
     * 根据道具的类型添加道具
     * @param userId        玩家id
     * @param itemType      道具类型
     * @param num           道具数量
     */
    public boolean addItem(int userId , int itemType , int num) {
        ItemEntity item = getItemByType(userId, itemType);
        boolean ret = false;
        if (item == null) {
            item = init(userId , itemType , num);
            ret = ItemDao.getInstance().addItem(userId, item);
            if(ret){
                pushChangeItem(userId , Arrays.asList(item));
            }
        } else {
            item.setNum(item.getNum() + num);
            ret = ItemDao.getInstance().updateItem(userId, item);
            if(ret){
                pushChangeItem(userId , Arrays.asList(item));
            }
        }
        return ret;
    }

    /**
     * 添加多种道具接口
     * @param userId
     * @param addMap    key：itemType ,  value : num
     * @return
     */
    public boolean addBatchItems(int userId , Map<Integer , Integer> addMap){
        if(addMap == null || addMap.keySet().size() == 0){
            return true;
        }
        List<ItemEntity> list = getAllUserItems(userId);
        List<ItemEntity> addItem = new ArrayList<>();
        List<ItemEntity> updateItem = new ArrayList<>();
        if(list.size() == 0){
            for(Integer itemType : addMap.keySet()) {
                int addNum = addMap.get(itemType);
                ItemEntity item = init(userId , itemType , addNum);
                addItem.add(item);
            }
        }else{
            for(Integer itemType : addMap.keySet()){
                int addNum = addMap.get(itemType);
                boolean has = false;
                for(ItemEntity entity : list){
                    if(entity.getItemType() == itemType){
                        has = true;
                        entity.setNum(entity.getNum() + addNum);
                        updateItem.add(entity);
                        break;
                    }
                }
                if(!has){
                    ItemEntity newItem = init(userId , itemType , addNum);
                    addItem.add(newItem);
                }
            }
        }
        boolean ret = false;
        if(addItem.size() > 0){
            ret = ItemDao.getInstance().insertBatchItems(userId , addItem);
            if(ret){
                pushChangeItem(userId , addItem);
            }
        }
        if(updateItem.size() > 0){
            ret = ItemDao.getInstance().updateBatchItems(userId , updateItem);
            if(ret){
                pushChangeItem(userId , updateItem);
            }
        }
        return ret;
    }

    /**
     * 判断是否有足够道具消耗
     * @param userId        玩家id
     * @param itemType      消耗的道具类型
     * @param num           消耗的道具的数量
     * @return
     */
    public boolean isEnoughItem(int userId , int itemType , int num){
        ItemEntity item = getItemByType(userId , itemType);
        if(item != null && item.getNum() >= num){
            return true;
        }
        return false;
    }

    /**
     * 构建返回客户端的道具builder
     * @param itemEntity        道具实体
     */
    public Hall.UserItem.Builder buildUserItem(ItemEntity itemEntity){
        Hall.UserItem.Builder builder = Hall.UserItem.newBuilder();
        builder.setAmount(itemEntity.getNum());
        builder.setType(itemEntity.getItemType());
        return builder;
    }

    /**
     * 消耗单个类型的道具
     * @param userId        用户id
     * @param itemType      消耗的道具类型
     * @param num           消耗的道具的数量
     */
    public boolean costItem(int userId , int itemType , int num){
        Map<Integer , Integer> map = new HashMap<>();
        map.put(itemType , num);
        return costBatchItem(userId , map);
    }


    /**
     * 消耗多个不同类型的道具
     * @param userId        用户ia
     * @param map           消耗的集合，形式为 道具类型：消耗数量
     * @return  true 消耗成功；false 失败
     */
    public boolean costBatchItem(int userId , Map<Integer , Integer> map){
        if(map == null || map.size() == 0){
            return true;
        }
        Set<Integer> keys = map.keySet();
        //先检查是否足够
        for(int itemType : keys){
            ItemEntity entity = getItemByType(userId , itemType);
            if(entity == null){
                return false;
            }
            if(entity.getNum() < map.get(itemType)){
                return false;
            }
        }
        //扣除数据
        List<ItemEntity> updateList = new ArrayList<>();
        for(int itemType : keys){
            ItemEntity entity = getItemByType(userId , itemType);
            int cur = entity.getNum() - map.get(itemType);
            cur = cur < 0 ? 0 : cur;
            entity.setNum(cur);
            updateList.add(entity);
        }
        if(updateList.size() > 0){
            boolean ret = ItemDao.getInstance().updateBatchItems(userId , updateList);
            if(ret){
                pushChangeItem(userId , updateList);
            }
        }
        return true;
    }


    private ItemEntity init(int userId , int itemType , int num){
        ItemEntity item = new ItemEntity();
        item.setItemType(itemType);
        item.setNum(num);
        item.setUserId(userId);
        return item;
    }

    /**
     * 道具发生变化，推送给前端
     * @param userId        用户id
     * @param entity        道具实体
     */
    public void pushChangeItem(int userId , List<ItemEntity> entity){
        Session userSession = GameData.getSessionManager().getSessionByUserId(userId);
        if(userSession != null){
            Hall.PushUserItem.Builder builder = Hall.PushUserItem.newBuilder();
            for(ItemEntity entity1 : entity) {
                Hall.UserItem.Builder itemBuilder = buildUserItem(entity1);
                builder.addUserItems(itemBuilder);
            }
            userSession.sendClient(Cmd.S2C_Hall_Push_Item , builder.build().toByteArray());
        }
    }

    public static void main(String[] args){
    }
}
