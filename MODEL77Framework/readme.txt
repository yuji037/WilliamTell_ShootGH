
���S��

	�E�ŏ��ɋN������V�[����GamemManager�ƂȂ�܂��B
	�E[Edit] > [Project Settings] > [Player]���J���AInspector��[Optimization]������[ApiCompatibility Level]��[.NET2.0 Subset]����[.NET2.0]�ɕύX���Ă��������B
	�EClass���A�^�O�����A�Q�[���I�u�W�F�N�g����G00_(�ȉ��Q�[��NO)�Ȃǂ̐ړ����t�����đ��̃Q�[���Ɠ�����Ȃ��悤�ɂ��Ă��������B
	�E�֘A�t�@�C���̓Q�[��NO�Ńt�H���_�����A�����ɂɔz�u���Ă��������B
	�EResources�𗘗p����ꍇ�̓Q�[���ԍ��Ńt�H���_�����A�����𗘗p���Ă��������B

	<�t�H���_�\��>
	��:G00
	�@Assets/
	�@�@�� G00/��*G00�pAssets
	�@�@�� G01/
	�@�@�� ...
	�@�@�� G09/
	�@�@�� GameManager/
	�@�@�� Resources/
	�@�@���@�� G00/��*G00�pResources
	�@�@���@�� G01/
	�@�@���@�� ...
	�@�@���@�� G09/
	�@�@�� StreamingAssets/
	�@�@�@�@�� node.js
	�@�@�@�@�� .node/
�@�@�@�@�@�@�@�@��server.js(SocketIO�T�[�o�[�v���O����)

	<Hierarchy�\��>
		[Scene:GameManager]
		��	GameManager �c �Q�[���V�[���̃��[�h/�A�����[�h���R���g���[�����܂��B
		��	SocketIO    �c �^�u���b�g�R���g���[���[�Ƃ�socket�ʐM�p�R���|�[�l���g�ł��B
		��	NodeJS	    �c node.js�̃v���Z�X�N���p�̃R���|�[�l���g�ł��B
		��[Scene:Game00]���̉��w�ɃQ�[�������[�h���܂�

�����W�֘A

	<�R���|�[�l���g>
	    CoordinateManager CM;    
	
	    void Start(){
	        
	        CM = GameObject.Find("GameManager").GetComponent<CoordinateManager>();
	    
	    }

	<���蕔��>
	    if (CM.isUpdate()) {//���e�m�F
	        Hashtable res = CM.Get(); //Get�����s����܂�isUpdate��true��Ԃ�������B
	       
		Vector3 pos = new Vector3((float)pos["x"], (float)pos["y"], 0f);

	<Hashtable�̒��g>
	    x:raycast�px���W
	    y:raycast�py���W
	    energy:�G�l���M�[(J)
	    speed:�e��(m/s)
	    hittim:��
	    ui_x:UGUI�ł�x���W
	    ui_y:UGUI�ł�y���W
	    st_x:�Z���T�[�̐�x���W(mm)
	    st_y:�Z���T�[�̐�y���W(mm)
	    sc_w:��ʂ̕�(px)
	    sc_h:��ʂ̍���(px)
	    se_w:�Z���T�[�̕�(mm)
	    se_h:�Z���T�[�̍���(mm)
	    adjust_x:���ݕ␳���Ă���x�����̗�(px)
	    adjust_y:���ݕ␳���Ă���y�����̗�(px)


���X�R�A�֘A
	
	�z��ŁA�v���C���[ID�A�X�R�A�A�v���C�I�����Ԃ��֐��ɓn���܂��B

	<��:�v���C���[���擾>
	GameController GC = GameObject.Find("GameManager").GetComponent<GameController>();
        string[] idm = new string[2];//�v���C���[ID
        string[] name = new string[2];//�v���C���[��
        int[] score = new int[2];//�X�R�A
        string[] idate = new string[2];//�v���C�I������
	
	//�v���C���[ID�A�v���C���[���̎擾
        int cnt = 0;
        for (int i=0;i<= GC.player_isentry.Length; i++)
        {
            if (GC.player_isentry[i] == true)//player_isentry��true�̐l���Q��
            {
                idm[cnt] = GC.player_id[i];
                name[cnt] = GC.player_id[i];
                cnt++;
            }
        }

	<��:�_���o�^>
	//���ݎ������擾
	idate[0] = GC.Now();
        idate[1] = idate[0];

        GC.ScoreUpdate(idm, score, idate);


���Q�[���I��

	���̃��\�b�h�����s����ƃQ�[���I���V�[�����Ăяo���܂��B
	GameObject.Find("GameManager").GetComponent<GameController>().GameEnd();


���^�u���b�g�R���g���[���[�֘A

	<�R���|�[�l���g>
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

	<�C�x���g���X�i�[>
        socket.On("/change", (SocketIOEvent e) => {
            Debug.Log(e);
            /*

                �`�F���W����

            */
        });

	<�C�x���g���M>
		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("error", 0);
        jsonObject.AddField("data", "���肽���f�[�^");
        socket.Emit("/change", jsonObject);

	<�C�x���g�ꗗ>
		/title       �c �^�C�g���V�[�����Ăяo���܂��B
		/start       �c �X�^�[�g��ʂŎ��s����ƃQ�[���Z���N�g�V�[�����Ăяo���܂��B
		/roll        �c ������ݒ肵�܂��B(machine/controller)
		/rollcheck   �c ���ݐݒ肳��Ă��������Ԃ��܂��B
		/demo        �c �f�����[�r�[���Đ����܂��B
		/isinput     �c ���͂̋���/�s��Ԃ��܂��B
		/calibration �c ���W�̃L�����u���[�V�����V�[�����Ăяo���܂��B
		/entry       �c �Q�����郆�[�U�[��I�����Ƃ��Đݒ肵�܂��B
		/join        �c /entry�̃��[�U�[���X�g����Q�[���ɎQ�����郆�[�U�[��ݒ肵�܂��B
		/game        �c �w�肳�ꂽ�Q�[�����Ăяo���܂��B

�����s��

	<�f�B�X�v���C>
		�T�C�Y:84�C���`
		��ʉ𑜓x:FULLHD(1920�~1080px)
		��ʐ��@:1864.8mm�~1050.8mm
		�c����:16:9

	<PC>
		OS:Win10
		CPU:i5-7600 3.5GHz
		������:DDR4 16GB
		HDD:SSD256GB
		�O���{:GTX1050Ti