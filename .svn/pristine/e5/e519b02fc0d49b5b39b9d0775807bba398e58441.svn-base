package avatar.module.room;

import avatar.module.room.dao.RoomDao;
import avatar.util.LogUtil;
import avatar.util.RandomUtil;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * 房间号接口
 */
public class GeneratorRoomCodeService {
    private static final GeneratorRoomCodeService instance = new GeneratorRoomCodeService();

    public static final GeneratorRoomCodeService getInstance() {
        return instance;
    }

    private final List<String> roomCodeList = new ArrayList<>();
    private final int initCount = 5000;//初始化n个roomCode在内存中
    private final int minCount = 500;//当内存中还剩下n个时，再次生成到initCount值

    private static final List<List<Integer>> intervalList = new ArrayList<>();

    /**
     * 初始化数组区间
     */
    public void initInterval(){
        int start = 100000;//总区间开始
        int end = 1000000;//总区间结束
        int num = 20000;//每个区间个数
        List<Integer> arr;
        for(int i = start + 1 ; i < end ; i++){
            if(intervalList.size() == 0){
                intervalList.add(new ArrayList<>());
            }
            arr = intervalList.get(intervalList.size() - 1);
            if(arr.size() >= num){
                Collections.shuffle(arr);
                arr = new ArrayList<>();
                arr.add(i);
                intervalList.add(arr);
            }else{
                arr.add(i);
            }
        }
    }

    public String getNextRoomCode() {
        String code = roomCodeList.remove(0);
        if(roomCodeList.size() <= minCount){
            initRoomCode();
        }
        return code;
    }


    public void initRoomCode() {
        List<String> curList = RoomDao.getInstance().loadAddRoomCode();
        int size = intervalList.size();
        int temp = 0;
        while (true){
            temp++;
            int index = RandomUtil.iRand(size);
            List<Integer> l = intervalList.get(index);
            int len = l.size();
            for(int i = 0; i < len ; i++){
                String v = String.valueOf(l.get(i));
                if(curList.indexOf(v) > -1){
                    continue;
                }
                roomCodeList.add(v);
            }
            if(roomCodeList.size() >= initCount){
                return;
            }
            if(temp >= 1000000){//极端情况
                LogUtil.getLogger().error("[GeneratorRoomCodeService]initRoomCode , init room code failed.");
                return;
            }
        }
    }

    public static void main(String[] args) {
        Runtime myRun = Runtime.getRuntime();
        System.out.println("已用内存" + myRun.totalMemory());
        System.out.println("最大内存" + myRun.maxMemory());
        System.out.println("可用内存" + myRun.freeMemory());
        long time1 = System.currentTimeMillis();
        GeneratorRoomCodeService.getInstance().initInterval();
        GeneratorRoomCodeService.getInstance().initRoomCode();
        long time2 = System.currentTimeMillis();
        System.out.println("执行此程序总共花费了" + ( time2 - time1 )+ "毫秒");
        System.out.println("已用内存" + myRun.totalMemory());
        System.out.println("最大内存" + myRun.maxMemory());
        System.out.println("可用内存" + myRun.freeMemory());
    }
}
